using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.ModuloMedicamento
{
    public class RepositorioMedicamentoEmBancoDados
    {
		private string EndereçoBanco = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ControleDeMedicamentosDb;Integrated Security=True;Pooling=False";

		private string SelecionarTodosMedicamentos = @"Select
												  m.id,
												  m.Nome as Nome_Medicamento,												  
												  m.Descricao,
												  m.Lote,
												  m.Validade,
												  m.QuantidadeDisponivel,

												  f.id as FORNECEDOR_ID,
												  f.Nome,
												  f.Telefone,
												  f.Email,
												  f.Cidade,
												  f.Estado  
   
												  From TBMedicamento as M  
												  inner join TBFornecedor as f on m.Forncedor_Id= f.Id 
";

		private string SelecionarMedicamentoPorNumero = @"Select  
												  m.id,
												  m.Nome as Nome_Medicamento,
												  m.Descricao,
												  m.Lote,
												  m.Validade,
												  m.QuantidadeDisponivel,

												  f.id as FORNECEDOR_ID,
												  f.nome,
												  f.Telefone,
												  f.Email,
												  f.Cidade,
												  f.Estado
     
												  From TBMedicamento as M  
												  inner join TBFornecedor as f on m.Forncedor_Id= f.Id 


												  WHERE
													 m.ID=@ID
";
		private string InserirMedicamento = @"Insert into [TBMedicamento]
												(
												  [NOME],
												  [DESCRICAO],
												  [LOTE],
												  [VALIDADE],
												  [QUANTIDADEDISPONIVEL],
												  [Forncedor_Id]												  
												 )
												  Values 
												 (
												  @NOME,
												  @DESCRICAO,
												  @LOTE,
												  @VALIDADE,
												  @QUANTIDADEDISPONIVEL,
												  @Forncedor_Id
												  );
												Select SCOPE_IDENTITY()";
		private string EditarMedicamento = @"UPDATE[TBMedicamento]
											SET
											   [NOME] = @NOME,
											   [DESCRICAO] = @DESCRICAO,
											   [LOTE]=@LOTE,
											   [VALIDADE]=@VALIDADE,
											   [QUANTIDADEDISPONIVEL]=@QUANTIDADEDISPONIVEL,
											   [Forncedor_Id]=@Forncedor_Id
											   
											WHERE
											   [ID] = @ID";
		private string ExcluirMedicamento = @"DELETE FROM [TBMedicamento]
													 WHERE 
														[ID]=@ID";
		private string SelecionarRequisicao = @"Select 
                                                QuantidadeMedicamento
												From TBRequisicao 
												 WHERE 
												  Medicamento_Id=@ID";

		public ValidationResult Inserir(Medicamento novoRegistro, Fornecedor fornecedor)
		{
			var validador = new ValidadorMedicamento();

			var resultadoValidacao = validador.Validate(novoRegistro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(InserirMedicamento, conexaoComBanco);

			ConfigurarParametrosMedicamento(novoRegistro, comandoInsercao,fornecedor);

			conexaoComBanco.Open();
			var id = comandoInsercao.ExecuteScalar();
			novoRegistro.Id = Convert.ToInt32(id);

			conexaoComBanco.Close();

			return resultadoValidacao;
		}

        private void ConfigurarParametrosMedicamento(Medicamento novoRegistro, SqlCommand comandoInsercao,Fornecedor fornecedor)
        {
			comandoInsercao.Parameters.AddWithValue("ID", novoRegistro.Id);
			comandoInsercao.Parameters.AddWithValue("NOME", novoRegistro.Nome);
			comandoInsercao.Parameters.AddWithValue("DESCRICAO", novoRegistro.Descricao);
			comandoInsercao.Parameters.AddWithValue("LOTE", novoRegistro.Lote);
			comandoInsercao.Parameters.AddWithValue("VALIDADE", novoRegistro.Validade);
			comandoInsercao.Parameters.AddWithValue("QUANTIDADEDISPONIVEL",novoRegistro.QuantidadeDisponivel);
			comandoInsercao.Parameters.AddWithValue("Forncedor_Id",fornecedor.Id);
		}
		public ValidationResult Editar(Medicamento registro,Fornecedor fornecedor)
		{
			var validador = new ValidadorMedicamento();

			var resultadoValidacao = validador.Validate(registro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoEdicao = new SqlCommand(EditarMedicamento, conexaoComBanco);

			ConfigurarParametrosMedicamento(registro, comandoEdicao,fornecedor);

			conexaoComBanco.Open();

			comandoEdicao.ExecuteNonQuery();

			conexaoComBanco.Close();

			return resultadoValidacao;
		}
		public ValidationResult Excluir(Medicamento registro)
		{
			var validador = new ValidadorMedicamento();

			var resultadoValidacao = validador.Validate(registro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoRemover = new SqlCommand(ExcluirMedicamento, conexaoComBanco);

			comandoRemover.Parameters.AddWithValue("ID", registro.Id);

			conexaoComBanco.Open();

			comandoRemover.ExecuteNonQuery();

			conexaoComBanco.Close();

			return resultadoValidacao;
		}
		public List<Medicamento> SelecionarTodos()
		{
			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(SelecionarTodosMedicamentos, conexaoComBanco);

			List<Medicamento> medicamentos = new List<Medicamento>();

			conexaoComBanco.Open();


			SqlDataReader leitor = comandoInsercao.ExecuteReader();

			while (leitor.Read())
			{
				Medicamento medicamento = ConverterMedicamento(leitor);

				medicamento=adicionarRequisicao(medicamento);

				medicamentos.Add(medicamento);
			}

			conexaoComBanco.Close();
			return medicamentos;
		}
		public Medicamento SelecionarMedicamentoNumero(int numero)
        {

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoSelecao = new SqlCommand(SelecionarMedicamentoPorNumero, conexaoComBanco);

			comandoSelecao.Parameters.AddWithValue("ID", numero);

			conexaoComBanco.Open();
			SqlDataReader leitorMedicamento = comandoSelecao.ExecuteReader();

			Medicamento medicamento = null;

			if (leitorMedicamento.Read())
            {
				medicamento = ConverterMedicamento(leitorMedicamento);
				medicamento = adicionarRequisicao(medicamento);
			}
			conexaoComBanco.Close();

			return medicamento;

		}

        public Medicamento ConverterMedicamento(SqlDataReader leitor)
        {
			int id = Convert.ToInt32(leitor["ID"]);
			var nome = Convert.ToString(leitor["Nome_Medicamento"]);
			var descricao = Convert.ToString(leitor["Descricao"]);
			var lote = Convert.ToString(leitor["Lote"]);
			var validade = Convert.ToDateTime(leitor["Validade"]);
			

			var idFornecedor =Convert.ToInt32(leitor["FORNECEDOR_ID"]);
			string name = Convert.ToString(leitor["NOME"]);
			string email = Convert.ToString(leitor["EMAIL"]);
			string estado = Convert.ToString(leitor["ESTADO"]);
			string cidade = Convert.ToString(leitor["CIDADE"]);
			string telefone = Convert.ToString(leitor["TELEFONE"]);

			var medicamento = new Medicamento(nome, descricao, lote, validade)
			{
				Id = id,
				Fornecedor = new Fornecedor(name, telefone, email, cidade, estado)
				{
					Id = idFornecedor
				}
			};
		   return medicamento;
		}
		public Medicamento adicionarRequisicao(Medicamento medicamento)
        {

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(SelecionarRequisicao, conexaoComBanco);

            comandoInsercao.Parameters.AddWithValue("ID", medicamento.Id);

			List<Requisicao> requisicoes = new List<Requisicao>();

			conexaoComBanco.Open();

			SqlDataReader leitor = comandoInsercao.ExecuteReader();
		

			while (leitor.Read())
            {
				Requisicao requisicao = ConverterParaRequisicao(leitor);

				requisicoes.Add(requisicao);
            }
			medicamento.Requisicoes = requisicoes;

			conexaoComBanco.Close();
			return medicamento;
        }

        private Requisicao ConverterParaRequisicao(SqlDataReader leitor)
        {
            int quantidade = Convert.ToInt32(leitor["QuantidadeMedicamento"]);

			var requisicao = new Requisicao
			{
				QtdMedicamento = quantidade
			};
			return requisicao;
        }
    }     
}
