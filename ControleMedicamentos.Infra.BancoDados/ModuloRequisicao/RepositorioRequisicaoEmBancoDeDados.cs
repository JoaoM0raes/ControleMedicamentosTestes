using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloRequisicao
{
    public  class RepositorioRequisicaoEmBancoDeDados
    {
		private string EndereçoBanco = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ControleDeMedicamentosDb;Integrated Security=True;Pooling=False";

		private string SelecionarTodasRequisicao = @"Select
                                                          r.id,
														  r.QuantidadeMedicamento,
														  r.Data,

                                                          p.id as paciente_id,
														  p.Nome as paciente_nome,
														  p.CartaoSUS,

                                                          f.id as funcionario_id,
														  f.Login,
														  f.Nome as funcionario_nome,
														  f.Senha,

                                                          m.id as medicamento_id, 
														  m.Descricao,
														  m.Nome,
														  m.Lote,
														  m.Validade
   
														From TBRequisicao as r  
														inner join TBPaciente as p on r.Paciente_Id= p.Id 
														inner join TBFuncionario as f on r.Funcionario_Id=f.Id
														inner join TBMedicamento as m on r.Medicamento_Id= m.Id
";

		private string SelecionarRequisicaoPorNumero = @"Select
                                                          r.id,
														  r.QuantidadeMedicamento,
														  r.Data,

                                                          p.id as paciente_id,
														  p.Nome paciente_nome,
														  p.CartaoSUS,

                                                          f.id as funcionario_id,
														  f.Login,
														  f.Nome as funcionario_nome,
														  f.Senha,

                                                          m.id as medicamento_id, 
														  m.Descricao,
														  m.Nome,
														  m.Lote,
														  m.Validade
   
														From TBRequisicao as r  
														inner join TBPaciente as p on r.Paciente_Id= p.Id 
														inner join TBFuncionario as f on r.Funcionario_Id=f.Id
														inner join TBMedicamento as m on r.Medicamento_Id= m.Id

														WHERE 
															r.ID=@ID;

";
		private string InserirRequisicao = @"Insert into [TBRequisicao]
												(
												  [DATA],
												  [QuantidadeMedicamento],
											      [Medicamento_Id],
												  [Paciente_Id],
												  [Funcionario_Id]
												  
												 )
												  Values 
												 (
												  @DATA,
												  @QuantidadeMedicamento,
												  @Medicamento_Id,
												  @Paciente_Id,
												  @Funcionario_Id

												  );
												Select SCOPE_IDENTITY()";
		private string EditarRequisicao = @"UPDATE[TBRequisicao]
											SET
											   [DATA]=@DATA,
											   [QuantidadeMedicamento] = @QuantidadeMedicamento,
											   [Medicamento_Id]=@Medicamento_Id,
											   [Paciente_Id]=@Paciente_Id,
											   [Funcionario_Id]=@Funcionario_Id
											WHERE
											   [ID] = @ID";
		private string ExcluirRequisicao = @"DELETE FROM [TBRequisicao]
													 WHERE 
														[ID]=@ID";

		public ValidationResult Inserir(Requisicao novoRegistro)
		{
			var validador = new ValidadorRequisicao();

			var resultadoValidacao = validador.Validate(novoRegistro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(InserirRequisicao, conexaoComBanco);

			ConfigurarParametrosRequisicao(novoRegistro, comandoInsercao);

			conexaoComBanco.Open();
			var id = comandoInsercao.ExecuteScalar();
			novoRegistro.Id = Convert.ToInt32(id);

			conexaoComBanco.Close();

			return resultadoValidacao;
		}

        private void ConfigurarParametrosRequisicao(Requisicao novoRegistro, SqlCommand comandoInsercao)
        {
			comandoInsercao.Parameters.AddWithValue("ID", novoRegistro.Id);
			comandoInsercao.Parameters.AddWithValue("DATA", novoRegistro.Data);
			comandoInsercao.Parameters.AddWithValue("QuantidadeMedicamento", novoRegistro.QtdMedicamento);
			comandoInsercao.Parameters.AddWithValue("Medicamento_Id", novoRegistro.Medicamento.Id);
			comandoInsercao.Parameters.AddWithValue("Paciente_Id", novoRegistro.Paciente.Id);
			comandoInsercao.Parameters.AddWithValue("Funcionario_Id", novoRegistro.Funcionario.Id);

		}
		public ValidationResult Editar(Requisicao novoRegistro)
        {
			var validador = new ValidadorRequisicao();

			var resultadoValidacao = validador.Validate(novoRegistro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(EditarRequisicao, conexaoComBanco);

			ConfigurarParametrosRequisicao(novoRegistro, comandoInsercao);

			conexaoComBanco.Open();

			comandoInsercao.ExecuteNonQuery();

			conexaoComBanco.Close();

			return resultadoValidacao;
		}
		public ValidationResult Excluir(Requisicao novoRegistro)
        {

			var validador = new ValidadorRequisicao();

			var resultadoValidacao = validador.Validate(novoRegistro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(ExcluirRequisicao, conexaoComBanco);

			comandoInsercao.Parameters.AddWithValue("ID", novoRegistro.Id);

			conexaoComBanco.Open();

			comandoInsercao.ExecuteNonQuery();

			conexaoComBanco.Close();

			return resultadoValidacao; 
		}

		public Requisicao SelecionarRequisicaoNumero(int numero)
        {

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(SelecionarRequisicaoPorNumero, conexaoComBanco);

			comandoInsercao.Parameters.AddWithValue("ID", numero);

			conexaoComBanco.Open(); 

			SqlDataReader leitor = comandoInsercao.ExecuteReader();

			Requisicao requisicao = new Requisicao();

            if (leitor.Read())
            {
				requisicao = ConverterRequisicao(leitor);
            }
			conexaoComBanco.Close();

			return requisicao;
		}

        private Requisicao ConverterRequisicao(SqlDataReader leitor)
        {
			var id = Convert.ToInt32(leitor["Id"]);
			var QuantidadeMedicamento = Convert.ToInt32(leitor["QuantidadeMedicamento"]);
			var Data = Convert.ToDateTime(leitor["Data"]);

			var paciente_id = Convert.ToInt32(leitor["paciente_id"]);
			var Nome = Convert.ToString(leitor["paciente_nome"]);
			var CartaoSUS = Convert.ToString(leitor["CartaoSUS"]);

			var funcionario_id = Convert.ToInt32(leitor["funcionario_id"]);
			var Login = Convert.ToString(leitor["Login"]);
			var name = Convert.ToString(leitor["funcionario_nome"]);
			var Senha = Convert.ToString(leitor["Senha"]);

			var medicamento_id=	Convert.ToInt32(leitor["medicamento_id"]);
			var Descricao=Convert.ToString(leitor["Descricao"]);
			var NomeMedicamento = Convert.ToString(leitor["Nome"]);
			var Lote = Convert.ToString(leitor["Lote"]);
			var Validade = Convert.ToDateTime(leitor["Validade"]);


			var requisicao = new Requisicao
			{
				Id = id,
				QtdMedicamento = QuantidadeMedicamento,
				Data = Data,

				Paciente = new Paciente(Nome, CartaoSUS)
				{
					Id = paciente_id
				},
				Funcionario = new Funcionario()
				{
					Senha = Senha,
					Nome = name,
					Login = Login,
					Id = paciente_id
				},
				Medicamento = new Medicamento(NomeMedicamento, Descricao, Lote, Validade)
				{
					Id = medicamento_id	
                }			  
			};
			return requisicao; 
        }
    }
}
