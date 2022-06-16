using ControleMedicamentos.Dominio.ModuloPaciente;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloPaciente
{
    public class RepositorioPacienteEmBancoDeDados
    {
		private string EndereçoBanco = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ControleDeMedicamentosDb;Integrated Security=True;Pooling=False";

		private string SelecionarTodosPaciente = @"SELECT * FROM [TBPaciente]";

		private string SelecionarPacientePorNumero = @"SELECT * FROM [TBPaciente]
                                                            WHERE 
                                                                [ID] = @ID";
		private string InserirPaciente = @"Insert into [TBPaciente]
												(
												  [NOME],
												  [CartaoSUS]
												  
												 )
												  Values 
												 (
												  @NOME,
												  @CartaoSUS												  
												  );
												Select SCOPE_IDENTITY()";
		private string EditarPaciente = @"UPDATE[TBPaciente]
											SET
											   [NOME] = @NOME,
											   [CartaoSUS] = @CartaoSUS
											   
											WHERE
											   [ID] = @ID";
		private string ExcluirPaciente = @"DELETE FROM [TBPaciente]
													 WHERE 
														[ID]=@ID";

		public ValidationResult Inserir(Paciente novoRegistro)
		{
			var validador = new ValidadorPaciente();

			var resultadoValidacao = validador.Validate(novoRegistro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(InserirPaciente, conexaoComBanco);

			ConfigurarParametrosPaciente(novoRegistro, comandoInsercao);

			conexaoComBanco.Open();
			var id = comandoInsercao.ExecuteScalar();
			novoRegistro.Id = Convert.ToInt32(id);

			conexaoComBanco.Close();

			return resultadoValidacao;
		}

        private void ConfigurarParametrosPaciente(Paciente novoRegistro, SqlCommand comandoInsercao)
        {
			comandoInsercao.Parameters.AddWithValue("ID",novoRegistro.Id);
            comandoInsercao.Parameters.AddWithValue("NOME",novoRegistro.Nome);
			comandoInsercao.Parameters.AddWithValue("CartaoSUS", novoRegistro.CartaoSUS);
		}

		public ValidationResult Editar(Paciente registro)
		{
			var validador = new ValidadorPaciente();

			var resultadoValidacao = validador.Validate(registro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoEdicao = new SqlCommand(EditarPaciente, conexaoComBanco);

			ConfigurarParametrosPaciente(registro, comandoEdicao);

			conexaoComBanco.Open();

			comandoEdicao.ExecuteNonQuery();

			conexaoComBanco.Close();

			return resultadoValidacao;
		}
		public ValidationResult Excluir(Paciente registro)
		{
			var validador = new ValidadorPaciente();

			var resultadoValidacao = validador.Validate(registro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoRemover = new SqlCommand(ExcluirPaciente, conexaoComBanco);

			comandoRemover.Parameters.AddWithValue("ID", registro.Id);

			conexaoComBanco.Open();

			comandoRemover.ExecuteNonQuery();

			conexaoComBanco.Close();

			return resultadoValidacao;
		}
		public List<Paciente> SelecionarTodos()
		{
			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(SelecionarTodosPaciente, conexaoComBanco);

			List<Paciente> pacientes = new List<Paciente>();

			conexaoComBanco.Open();


			SqlDataReader leitor = comandoInsercao.ExecuteReader();

			while (leitor.Read())
			{
				Paciente paciente = ConverterPaciente(leitor);

				pacientes.Add(paciente);
			}

			conexaoComBanco.Close();
			return pacientes;
		}

        private Paciente ConverterPaciente(SqlDataReader leitor)
        {

			int id = Convert.ToInt32(leitor["ID"]);
			string nome = Convert.ToString(leitor["NOME"]);
			string sus = Convert.ToString(leitor["CartaoSUS"]);


			var Paciente = new Paciente(nome, sus)
			{
				Id = id,
			};
		

			return Paciente;
		}
		public Paciente SelecionarPacienteNumero(int numero)
		{
			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoSelecao = new SqlCommand(SelecionarPacientePorNumero, conexaoComBanco);

			comandoSelecao.Parameters.AddWithValue("ID", numero);

			conexaoComBanco.Open();
			SqlDataReader leitorPaciente = comandoSelecao.ExecuteReader();

			Paciente paciente = null;

			if (leitorPaciente.Read())
				paciente = ConverterPaciente(leitorPaciente);

			conexaoComBanco.Close();

			return paciente;
		}
	}
}
