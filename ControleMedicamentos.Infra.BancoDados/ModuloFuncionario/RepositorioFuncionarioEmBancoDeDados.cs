using ControleMedicamentos.Dominio.ModuloFuncionario;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFuncionario
{
    public class RepositorioFuncionarioEmBancoDeDados
    {
		private string EndereçoBanco = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ControleDeMedicamentosDb;Integrated Security=True;Pooling=False";

		private string SelecionarTodosFuncionario = @"SELECT * FROM [TBFuncionario]";

        private string SelecionarFumcionarioPorNumero = @"SELECT * FROM [TBFuncionario]
                                                            WHERE 
                                                                [ID] = @ID ";
        private string InserirFuncionario = @"Insert into [TBFuncionario]
												(
												  [NOME],
												  [LOGIN],
												  [SENHA]
												 )
												  Values 
												 (
												  @NOME,
												  @LOGIN,
												  @SENHA 
												  );
												Select SCOPE_IDENTITY()";
		private string EditarFuncionario = @"UPDATE[TBFuncionario]
											SET
											   [NOME] = @NOME,
											   [LOGIN] = @LOGIN,
											   [SENHA] = @SENHA
											WHERE
											   [ID] = @ID";
		private string ExcluirFuncionario = @"DELETE FROM [TBFuncionario]
													 WHERE 
														[ID]=@ID

";
		public ValidationResult Inserir(Funcionario novoRegistro)
		{
			var validador = new ValidadorFuncionario();

			var resultadoValidacao = validador.Validate(novoRegistro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(InserirFuncionario, conexaoComBanco);

			ConfigurarParametrosFuncionario(novoRegistro, comandoInsercao);

			conexaoComBanco.Open();
			var id = comandoInsercao.ExecuteScalar();
			novoRegistro.Id = Convert.ToInt32(id);

			conexaoComBanco.Close();

			return resultadoValidacao;
		}

        private void ConfigurarParametrosFuncionario(Funcionario novoRegistro, SqlCommand comandoInsercao)
        {
			comandoInsercao.Parameters.AddWithValue("ID", novoRegistro.Id);
			comandoInsercao.Parameters.AddWithValue("NOME", novoRegistro.Nome);
			comandoInsercao.Parameters.AddWithValue("LOGIN", novoRegistro.Login);
			comandoInsercao.Parameters.AddWithValue("SENHA", novoRegistro.Senha);
			
		}
		public ValidationResult Editar(Funcionario registro)
		{
			var validador = new ValidadorFuncionario();

			var resultadoValidacao = validador.Validate(registro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoEdicao = new SqlCommand(EditarFuncionario, conexaoComBanco);

			ConfigurarParametrosFuncionario(registro, comandoEdicao);

			conexaoComBanco.Open();

			comandoEdicao.ExecuteNonQuery();

			conexaoComBanco.Close();
						
			return resultadoValidacao;
		}
		public ValidationResult Excluir(Funcionario registro)
		{
			var validador = new ValidadorFuncionario();

			var resultadoValidacao = validador.Validate(registro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoRemover = new SqlCommand(ExcluirFuncionario, conexaoComBanco);

			comandoRemover.Parameters.AddWithValue("ID", registro.Id);			

			conexaoComBanco.Open();

			comandoRemover.ExecuteNonQuery();

			conexaoComBanco.Close();

			return resultadoValidacao;
		}
		public List<Funcionario> SelecionarTodos()
		{
			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(SelecionarTodosFuncionario, conexaoComBanco);

			List<Funcionario> funcionarios = new List<Funcionario>();

			conexaoComBanco.Open();


			SqlDataReader leitor = comandoInsercao.ExecuteReader();

			while (leitor.Read())
			{
				Funcionario funcionario = ConverterFuncionario(leitor);

				funcionarios.Add(funcionario);
			}

			conexaoComBanco.Close();
			return funcionarios;
		}
		

        private Funcionario ConverterFuncionario(SqlDataReader leitor)
        {
			int id = Convert.ToInt32(leitor["ID"]);
			string nome = Convert.ToString(leitor["NOME"]);
			string senha = Convert.ToString(leitor["SENHA"]);
			string login= Convert.ToString(leitor["LOGIN"]);

			var Funcionario = new Funcionario
			{
				Nome = nome,
				Id = id,
				Login = login,
				Senha = senha
			};
			
			return Funcionario;
		}
		public Funcionario SelecionarFuncionarioPorNumero(int numero)
		{
			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoSelecao = new SqlCommand(SelecionarFumcionarioPorNumero, conexaoComBanco);

			comandoSelecao.Parameters.AddWithValue("ID", numero);

			conexaoComBanco.Open();
			SqlDataReader leitorFuncionario = comandoSelecao.ExecuteReader();

			Funcionario Funcionario = null;

			if (leitorFuncionario.Read())
				Funcionario = ConverterFuncionario(leitorFuncionario);

			conexaoComBanco.Close();
			
			return Funcionario;
		}
	}
}
