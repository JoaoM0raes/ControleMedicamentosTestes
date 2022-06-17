using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using System;
using System.Data.SqlClient;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using GeradorClasses;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloMedicamento
{
    [TestClass]
    public class RepositorioMedicamentoEmBancoDadosTest
    {
        RepositorioMedicamentoEmBancoDados repositorio= new RepositorioMedicamentoEmBancoDados();

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
            Medicamento medicamento = gerador.GerandoMedicamento();

            Assert.IsNotNull(medicamento);   
        }
        [TestMethod]
        public void Deve_Editar_Medicamento()
        {
            Medicamento medicamento = gerador.GerandoMedicamento();

            medicamento.Nome = "bbbbbbbbbbb";

            repositorio.Editar(medicamento,medicamento.Fornecedor);

            var medicamentoEditado = repositorio.SelecionarMedicamentoNumero(medicamento.Id);

            Assert.AreEqual(medicamento.Nome,medicamentoEditado.Nome);


        }
        [TestMethod]
        public void Deve_Excluir_Medicamento()
        {
           Medicamento medicamento =gerador.GerandoMedicamento();

           repositorio.Excluir(medicamento);

            var MedicamentoExcluido = repositorio.SelecionarMedicamentoNumero(medicamento.Id);

            Assert.IsNull(MedicamentoExcluido);
         

        }
        [TestMethod]
        public void Deve_Selecionar_Todos()
        {
            Medicamento Medicamento = gerador.GerandoMedicamento();

            Medicamento MedicamentoDois = gerador.GerandoMedicamento();

            Medicamento MedicamentoTres = gerador.GerandoMedicamento();

            var MedicamentoList = repositorio.SelecionarTodos();

            for (int i = 0; i < MedicamentoList.Count; i++)
            {
                Assert.AreEqual(Medicamento.Nome, MedicamentoList[i].Nome);
            }

        }
        
        
    }
}
