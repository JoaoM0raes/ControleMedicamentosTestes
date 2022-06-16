using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloFornecedor
{
    [TestClass]
    public class RepositorioFornecedorEmBancoDadosTest
    {
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
            var repositorio = new RepositorioFornecedorEmBancoDeDados();
            var novoFornecedor = new Fornecedor("aaaa","bbbb","ccccc","ddddd","eeeeeee");
            repositorio.Inserir(novoFornecedor);

            var Fornecedor= repositorio.SelecionarFornecedorPorNumero(novoFornecedor.Id);
            
            Assert.AreEqual(novoFornecedor.Id, Fornecedor.Id);
            Assert.AreEqual(novoFornecedor.Nome, Fornecedor.Nome);
            Assert.AreEqual(novoFornecedor.Email, Fornecedor.Email);
            Assert.AreEqual(novoFornecedor.Telefone,Fornecedor.Telefone);
            Assert.AreEqual(novoFornecedor.Cidade, Fornecedor.Cidade);
        }
        [TestMethod]
        public void Deve_Editar_Fornecedor()
        {
           var repositorio= new  RepositorioFornecedorEmBancoDeDados();

           var novoFornecedor = new Fornecedor("Wanderson", "DePena", "Bustos", "Taison", "Alemao");

           repositorio.Inserir(novoFornecedor);

           Fornecedor fornecedor = repositorio.SelecionarFornecedorPorNumero(novoFornecedor.Id);

            fornecedor.Nome = "Inter";
            fornecedor.Email = "3x1";
            fornecedor.Telefone = "1909";
            fornecedor.Cidade = "Porto Alegre";
            fornecedor.Estado = "Rio grande do sul";

            repositorio.Editar(fornecedor);

            Fornecedor fornecedorEditado = repositorio.SelecionarFornecedorPorNumero(fornecedor.Id);

            Assert.AreEqual(fornecedor.Nome,fornecedorEditado.Nome);
            Assert.AreEqual(fornecedor.Email, fornecedorEditado.Email);
            Assert.AreEqual(fornecedor.Telefone, fornecedorEditado.Telefone);
            Assert.AreEqual(fornecedor.Cidade, fornecedorEditado.Cidade);
            Assert.AreEqual(fornecedor.Estado, fornecedorEditado.Estado);
        }
        [TestMethod]
        public void Deve_Excluir_Fornecedor()
        {
            var repositorio = new RepositorioFornecedorEmBancoDeDados();

            var novoFornecedor = new Fornecedor("Wanderson", "DePena", "Bustos", "Taison", "Alemao");

            repositorio.Excluir(novoFornecedor);

            var funcionarioExcluido = repositorio.SelecionarFornecedorPorNumero(novoFornecedor.Id);

            Assert.IsNull(funcionarioExcluido);
        } 
        public void Deve_Selecionar_Todos()
        {
            var repositorio = new RepositorioFornecedorEmBancoDeDados();

            var fornecedorUm = new Fornecedor("Wanderson", "DePena", "Bustos", "Taison", "Alemao");
            var fornecedorDois = new Fornecedor("Alemao", "Wanderson", "DePena", "Bustos", "Taison");
            var fornecedorTres = new Fornecedor("Jhonny", "Wanderson", "DePena", "Bustos", "Taison");

            repositorio.Inserir(fornecedorUm);
            repositorio.Inserir(fornecedorDois);
            repositorio.Inserir(fornecedorTres);

            var listaFornecedores = repositorio.SelecionarTodos();

            Assert.AreEqual(fornecedorUm.Nome, listaFornecedores[0].Nome);
            Assert.AreEqual(fornecedorDois.Nome, listaFornecedores[1].Nome);
            Assert.AreEqual(fornecedorTres.Nome, listaFornecedores[2].Nome);
        }
    }
}
