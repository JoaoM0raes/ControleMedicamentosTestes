using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using GeradorClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest
    {

        RepositorioFornecedorEmBancoDeDados repositorio = new RepositorioFornecedorEmBancoDeDados();

        GeradorDeClasses gerador= new GeradorDeClasses();
        private string sql = @"DELETE FROM [TBRequisicao]
                                     DBCC CHECKIDENT (TBRequisicao, RESEED, 0)

                                DELETE FROM [TBMedicamento]
                                    DBCC CHECKIDENT (TBMedicamento, RESEED, 0)

                               DELETE FROM [TBFuncionario]
                                    DBCC CHECKIDENT (TBFuncionario, RESEED, 0)

                               DELETE FROM [TBPaciente]
                                    DBCC CHECKIDENT (TBPaciente, RESEED, 0)

                                DELETE FROM [TBFornecedor]
                                     DBCC CHECKIDENT (TBFornecedor, RESEED, 0)

";
        private string EndereçoBanco = @"Data Source = (LocalDB)\MSSQLLocalDB;Initial Catalog = ControleDeMedicamentosDb; Integrated Security = True; Pooling=False";
        public RepositorioFornecedorEmBancoDadosTest()
        {
            SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

            SqlCommand comando = new SqlCommand(sql, conexaoComBanco);

            conexaoComBanco.Open();

            comando.ExecuteNonQuery();

            conexaoComBanco.Close();
        }
        [TestMethod]
        public void Deve_inserir_Fornecedor()
        {
            Fornecedor fornecedor = gerador.GerandoFornecedor();

            Assert.IsNotNull(fornecedor);
        }
        [TestMethod]
        public void Deve_Editar_Fornecedor()
        {
           Fornecedor fornecedor= gerador.GerandoFornecedor();

           fornecedor.Nome = "ardafadadadadada";

           repositorio.Editar(fornecedor);

           var fornecedorEditado = repositorio.SelecionarFornecedorPorNumero(fornecedor.Id);

            Assert.AreEqual(fornecedor.Nome,fornecedorEditado.Nome);


        }
        [TestMethod]
        public void Deve_Excluir_Fornecedor()
        {
            Fornecedor fornecedor = gerador.GerandoFornecedor();

            repositorio.Excluir(fornecedor);

            var fornecedorExcluido = repositorio.SelecionarFornecedorPorNumero(fornecedor.Id);

            Assert.IsNull(fornecedorExcluido);  
        }
        [TestMethod]
        public void Deve_Selecionar_Todos()
        {
            Fornecedor fornecedorUm = gerador.GerandoFornecedor();

            Fornecedor fornecedorDois = gerador.GerandoFornecedor();

            Fornecedor fornecedorTres = gerador.GerandoFornecedor();

            var listaFornecedor = repositorio.SelecionarTodos();

            for(int i = 0; i < listaFornecedor.Count; i++)
            {
                Assert.AreEqual(fornecedorUm.Nome, listaFornecedor[i].Nome);
            }  
        }
    }
}
