using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {

        private string sql = @"DELETE FROM [TBMedicamento]
                                 DBCC CHECKIDENT (TBMedicamento, RESEED, 0)

                               DELETE FROM [TBFornecedor]
                                    DBCC CHECKIDENT (TBFornecedor, RESEED, 0)
";
        

        private string EndereçoBanco = @"Data Source = (LocalDB)\MSSQLLocalDB;Initial Catalog = ControleDeMedicamentosDb; Integrated Security = True; Pooling=False";
        public RepositorioMedicamentoEmBancoDadosTest()
        {
            SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

            SqlCommand comando = new SqlCommand(sql, conexaoComBanco);            

            conexaoComBanco.Open();
           
            comando.ExecuteNonQuery();  
            
            conexaoComBanco.Close();
            
        }
        [TestMethod]
        public void Deve_inserir_medicamento_Com_Fornecedor()
        {
            Medicamento medicamento = new Medicamento("Amoxilina","Para dor","123133",DateTime.Today);

            Fornecedor fornecedor = new Fornecedor("AAA","2424242","AAA@GMAIL.COM","LAGES","SC");

            var repositorio = new RepositorioMedicamentoEmBancoDados();

            var repositorioFornecedor = new RepositorioFornecedorEmBancoDeDados();

            repositorioFornecedor.Inserir(fornecedor);

            var fornecedorComId = repositorioFornecedor.SelecionarFornecedorPorNumero(fornecedor.Id);

            repositorio.Inserir(medicamento,fornecedorComId);

            var medicamentoNovo=repositorio.SelecionarMedicamentoNumero(medicamento.Id);

            Assert.AreEqual(medicamento.Id, medicamentoNovo.Id);
            Assert.AreEqual(medicamento.Descricao, medicamentoNovo.Descricao);
            Assert.AreEqual(medicamento.Validade, medicamentoNovo.Validade);
            Assert.AreEqual(fornecedor.Nome, medicamentoNovo.Fornecedor.Nome);
        }
        [TestMethod]
        public void Deve_Editar_Medicamento()
        {
            Medicamento medicamento = new Medicamento("Amoxilina", "Para dor", "123133", DateTime.Today);

            Fornecedor fornecedor = new Fornecedor("AAA", "2424242", "AAA@GMAIL.COM", "LAGES", "SC");

            var repositorio = new RepositorioMedicamentoEmBancoDados();

            var repositorioFornecedor = new RepositorioFornecedorEmBancoDeDados();

            repositorioFornecedor.Inserir(fornecedor);

            var fornecedorComId = repositorioFornecedor.SelecionarFornecedorPorNumero(fornecedor.Id);

            repositorio.Inserir(medicamento, fornecedorComId);

            var medicamentoNovo = repositorio.SelecionarMedicamentoNumero(medicamento.Id);

            medicamentoNovo.Nome = "PAO";
            medicamentoNovo.Descricao="Frances";

            repositorio.Editar(medicamentoNovo,fornecedor);

            var medicamentoEditado = repositorio.SelecionarMedicamentoNumero(medicamentoNovo.Id);

             Assert.AreEqual(medicamentoNovo.Nome, medicamentoEditado.Nome);
            Assert.AreEqual(medicamentoNovo.Descricao, medicamentoEditado.Descricao);


        }
        [TestMethod]
        public void Deve_Excluir_Medicamento()
        {
            Medicamento medicamento = new Medicamento("Amoxilina", "Para dor", "123133", DateTime.Today);

            Fornecedor fornecedor = new Fornecedor("AAA", "2424242", "AAA@GMAIL.COM", "LAGES", "SC");

            var repositorio = new RepositorioMedicamentoEmBancoDados();

            var repositorioFornecedor = new RepositorioFornecedorEmBancoDeDados();

            repositorioFornecedor.Inserir(fornecedor);

            var fornecedorComId = repositorioFornecedor.SelecionarFornecedorPorNumero(fornecedor.Id);

            repositorio.Inserir(medicamento, fornecedorComId);

            var medicamentoNovo = repositorio.SelecionarMedicamentoNumero(medicamento.Id);

            repositorio.Excluir(medicamentoNovo);

            var medicamentoExcluido = repositorio.SelecionarMedicamentoNumero(medicamentoNovo.Id);

            Assert.IsNull(medicamentoExcluido);

        }
        [TestMethod]
        public void Deve_Selecionar_Todos()
        {
            Medicamento medicamento = new Medicamento("Amoxilina", "Para dor", "123133", DateTime.Today);

            Medicamento medicamentoDois = new Medicamento("Painkiller", "Para dor", "123133", DateTime.Today);

            Medicamento medicamentoTres = new Medicamento("vasco", "Para dor", "123133", DateTime.Today);

            Fornecedor fornecedor = new Fornecedor("AAA", "2424242", "AAA@GMAIL.COM", "LAGES", "SC");

            var repositorio = new RepositorioMedicamentoEmBancoDados();

            var repositorioFornecedor = new RepositorioFornecedorEmBancoDeDados();

            repositorioFornecedor.Inserir(fornecedor);

            var fornecedorComId = repositorioFornecedor.SelecionarFornecedorPorNumero(fornecedor.Id);

            repositorio.Inserir(medicamento, fornecedorComId);

            repositorio.Inserir(medicamentoDois, fornecedorComId);

            repositorio.Inserir(medicamentoTres, fornecedorComId);

            var listaMedicamento = repositorio.SelecionarTodos();

            Assert.AreEqual("Amoxilina", listaMedicamento[0].Nome);
            Assert.AreEqual("Painkiller", listaMedicamento[1].Nome);
            Assert.AreEqual("vasco", listaMedicamento[2].Nome);



        }
    }
}
