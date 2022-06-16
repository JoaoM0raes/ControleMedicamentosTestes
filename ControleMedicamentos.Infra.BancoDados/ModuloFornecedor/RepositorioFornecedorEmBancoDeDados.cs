using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.ModuloFornecedor
{
    public class RepositorioFornecedorEmBancoDeDados
    {
		private string EndereçoBanco = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ControleDeMedicamentosDb;Integrated Security=True;Pooling=False";

		private string SelecionarTodosForncedores = @"SELECT * FROM [TBFornecedor]";

		private string SelecionarForncedoresPorNumero = @"SELECT * FROM [TBFornecedor]
                                                            WHERE 
                                                                [ID] = @ID ";
		private string InserirForncedores = @"Insert into [TBFornecedor]
												(
												  [NOME],
												  [TELEFONE],
												  [EMAIL],
												  [CIDADE],
												  [ESTADO]
												 )
												  Values 
												 (
												  @NOME,
												  @TELEFONE,
												  @EMAIL,
												  @CIDADE,
												  @ESTADO
												  );
												Select SCOPE_IDENTITY()";
		private string EditarForncedores = @"UPDATE[TBFornecedor]
											SET
											   [NOME] = @NOME,
											   [TELEFONE] = @TELEFONE,
											   [EMAIL] = @EMAIL,
											   [CIDADE]=@CIDADE,
											   [ESTADO]=@ESTADO  
											WHERE
											   [ID] = @ID";
		private string ExcluirForncedores = @"DELETE FROM [TBFornecedor]
													 WHERE 
														[ID]=@ID";
		public ValidationResult Inserir(Fornecedor novoRegistro)
		{
			var validador = new ValidadorFornecedor();

			var resultadoValidacao = validador.Validate(novoRegistro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(InserirForncedores, conexaoComBanco);

			ConfigurarParametrosFornecedor(novoRegistro, comandoInsercao);

			conexaoComBanco.Open();
			var id = comandoInsercao.ExecuteScalar();
			novoRegistro.Id = Convert.ToInt32(id);

			conexaoComBanco.Close();

			return resultadoValidacao;
		}

        private void ConfigurarParametrosFornecedor(Fornecedor novoRegistro, SqlCommand comandoInsercao)
        {
			comandoInsercao.Parameters.AddWithValue("ID", novoRegistro.Id);
			comandoInsercao.Parameters.AddWithValue("NOME", novoRegistro.Nome);
			comandoInsercao.Parameters.AddWithValue("EMAIL", novoRegistro.Email);
			comandoInsercao.Parameters.AddWithValue("ESTADO", novoRegistro.Estado);
			comandoInsercao.Parameters.AddWithValue("CIDADE", novoRegistro.Cidade);
			comandoInsercao.Parameters.AddWithValue("TELEFONE", novoRegistro.Telefone);
		}
		public ValidationResult Editar(Fornecedor registro)
		{
			var validador = new ValidadorFornecedor();

			var resultadoValidacao = validador.Validate(registro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoEdicao = new SqlCommand(EditarForncedores, conexaoComBanco);

			ConfigurarParametrosFornecedor(registro, comandoEdicao);

			conexaoComBanco.Open();

			comandoEdicao.ExecuteNonQuery();

			conexaoComBanco.Close();

			return resultadoValidacao;
		}
		public ValidationResult Excluir(Fornecedor registro)
		{
			var validador = new ValidadorFornecedor();

			var resultadoValidacao = validador.Validate(registro);

			if (resultadoValidacao.IsValid == false)
				return resultadoValidacao;

			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoRemover = new SqlCommand(ExcluirForncedores, conexaoComBanco);

			comandoRemover.Parameters.AddWithValue("ID", registro.Id);

			conexaoComBanco.Open();

			comandoRemover.ExecuteNonQuery();

			conexaoComBanco.Close();

			return resultadoValidacao;
		}
		public List<Fornecedor> SelecionarTodos()
		{
			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoInsercao = new SqlCommand(SelecionarTodosForncedores, conexaoComBanco);

			List<Fornecedor> fornecedores = new List<Fornecedor>();

			conexaoComBanco.Open();


			SqlDataReader leitor = comandoInsercao.ExecuteReader();

			while (leitor.Read())
			{
				Fornecedor fornecedor = ConverterFornecedor(leitor);

				fornecedores.Add(fornecedor);
			}

			conexaoComBanco.Close();
			return fornecedores;
		}

        private Fornecedor ConverterFornecedor(SqlDataReader leitor)
        {
			int id = Convert.ToInt32(leitor["ID"]);
			string nome = Convert.ToString(leitor["NOME"]);
			string email = Convert.ToString(leitor["EMAIL"]);
			string estado = Convert.ToString(leitor["ESTADO"]);
			string cidade = Convert.ToString(leitor["CIDADE"]);
			string telefone = Convert.ToString(leitor["TELEFONE"]);

			var fornecedor = new Fornecedor(nome, telefone, email, cidade, estado)
			{
				Id = id,
			};
			return fornecedor;
		}
		public Fornecedor SelecionarFornecedorPorNumero(int numero)
		{
			SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

			SqlCommand comandoSelecao = new SqlCommand(SelecionarForncedoresPorNumero, conexaoComBanco);

			comandoSelecao.Parameters.AddWithValue("ID", numero);

			conexaoComBanco.Open();
			SqlDataReader leitorFornecedor = comandoSelecao.ExecuteReader();

			Fornecedor fornecedor = null;

			if (leitorFornecedor.Read())
				fornecedor = ConverterFornecedor(leitorFornecedor);

			conexaoComBanco.Close();

			return fornecedor;
		}
	}
}
