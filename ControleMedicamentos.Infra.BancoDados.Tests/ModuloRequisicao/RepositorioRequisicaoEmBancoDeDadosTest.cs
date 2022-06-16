using ControleMedicamento.Infra.BancoDados.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using ControleMedicamentos.Infra.BancoDados.ModuloFornecedor;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloRequisicao;
using GeradorClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Tests.ModuloRequisicao
{
    [TestClass]
    public class RepositorioRequisicaoEmBancoDeDadosTest
    {
        RepositorioRequisicaoEmBancoDeDados repositorioRequisicao = new RepositorioRequisicaoEmBancoDeDados();
        GeradorDeClasses gerador;
        private string sql = @"
                                DELETE FROM [TBRequisicao]
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
        public RepositorioRequisicaoEmBancoDeDadosTest()
        {
            SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

            SqlCommand comando = new SqlCommand(sql, conexaoComBanco);

            conexaoComBanco.Open();

            comando.ExecuteNonQuery();

            conexaoComBanco.Close();

            gerador= new GeradorDeClasses();


        }
        [TestMethod]
        public void Deve_Inserir_Requisicao()
        {
           Requisicao requisicao=  gerador.GerandoRequisicao();
           
           var requisicaoNova =repositorioRequisicao.SelecionarRequisicaoNumero(requisicao.Id);

            Assert.AreEqual(requisicaoNova.Id, requisicao.Id);
            Assert.AreEqual(requisicaoNova.Paciente.Nome, requisicao.Paciente.Nome);

        }
        [TestMethod]
        public void Deve_Editar_Requisicao()
        {
            Requisicao requisicao = gerador.GerandoRequisicao();            

            var requisicaoNova = repositorioRequisicao.SelecionarRequisicaoNumero(requisicao.Id);

            Paciente paciente = gerador.GerandoPaciente();

            requisicaoNova.Paciente = paciente;

            repositorioRequisicao.Editar(requisicaoNova);

            var requisicaoEditada = repositorioRequisicao.SelecionarRequisicaoNumero(requisicaoNova.Id);

            Assert.AreEqual(requisicaoEditada.Paciente.Nome, requisicaoNova.Paciente.Nome);

        }
        [TestMethod]
        public void Deve_Excluir()
        {
            Requisicao requisicao=gerador.GerandoRequisicao();

            repositorioRequisicao.Excluir(requisicao);

            var requisicaoExcluida = repositorioRequisicao.SelecionarRequisicaoNumero(requisicao.Id);

            Assert.IsNull(requisicaoExcluida.Funcionario);
        }

        [TestMethod]
        public void Deve_Selecionar_Todos()
        {           
            Requisicao requisicao = gerador.GerandoRequisicao();

            Requisicao requisicaoDois = gerador.GerandoRequisicao();

            Requisicao requisicaoTres = gerador.GerandoRequisicao();

            var  requisicaoList = repositorioRequisicao.SelecionarTodos();

            for (int i = 0; i < requisicaoList.Count; i++)
            {
                Assert.AreEqual(requisicao.QtdMedicamento, requisicaoList[i].QtdMedicamento);
            }
        }

    }
}
