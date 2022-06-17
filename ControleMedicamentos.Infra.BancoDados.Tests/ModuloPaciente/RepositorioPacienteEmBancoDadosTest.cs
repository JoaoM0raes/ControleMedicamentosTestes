using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Collections.Generic;
using GeradorClasses;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]
    public class RepositorioPacienteEmBancoDadosTest
    {
        RepositorioPacienteEmBancoDeDados repositorioPaciente = new RepositorioPacienteEmBancoDeDados();
        GeradorDeClasses gerador = new GeradorDeClasses();
        private string sql = @"DELETE FROM [TBRequisicao]
                                     DBCC CHECKIDENT (TBRequisicao, RESEED, 0)

                                DELETE FROM [TBMedicamento]
                                    DBCC CHECKIDENT (TBMedicamento, RESEED, 0)

                               DELETE FROM [TBFuncionario]
                                    DBCC CHECKIDENT (TBFuncionario, RESEED, 0)

                               DELETE FROM [TBPaciente]
                                    DBCC CHECKIDENT (TBPaciente, RESEED, 0)

                                DELETE FROM [TBFornecedor]
                                     DBCC CHECKIDENT (TBFornecedor, RESEED, 0)";
        private string EndereçoBanco = @"Data Source = (LocalDB)\MSSQLLocalDB;Initial Catalog = ControleDeMedicamentosDb; Integrated Security = True; Pooling=False";
       public  RepositorioPacienteEmBancoDadosTest()
        {
            SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

            SqlCommand comando = new SqlCommand(sql, conexaoComBanco);

            conexaoComBanco.Open();

            comando.ExecuteNonQuery();

            conexaoComBanco.Close();
        }

       [TestMethod]
        public void Deve_inserir_Paciente()
        {
            Paciente paciente= gerador.GerandoPaciente();

            Assert.IsNotNull(paciente);
        }
        [TestMethod]
        public void Deve_Editar_Paciente()
        {
            Paciente paciente = gerador.GerandoPaciente();

            var pacienteNovo = repositorioPaciente.SelecionarPacienteNumero(paciente.Id);

            pacienteNovo.Nome = "Carlos";
            pacienteNovo.CartaoSUS = "2424242";

            repositorioPaciente.Editar(pacienteNovo);

            var pacienteEditado = repositorioPaciente.SelecionarPacienteNumero(pacienteNovo.Id);

            Assert.AreEqual(pacienteEditado.Nome, pacienteNovo.Nome);   
            Assert.AreEqual(pacienteEditado.CartaoSUS,pacienteNovo.CartaoSUS);


        }
        [TestMethod]
        public void Deve_Excluir_Paciente()
        {
            Paciente pacienteNovo = gerador.GerandoPaciente();

            repositorioPaciente.Excluir(pacienteNovo);

            var pacienteExcluido = repositorioPaciente.SelecionarPacienteNumero(pacienteNovo.Id);

            Assert.IsNull(pacienteExcluido);
        }
        [TestMethod]
        public void Deve_Selecionar_Todos()
        {
            Paciente Paciente = gerador.GerandoPaciente();

            Paciente PacienteDois = gerador.GerandoPaciente();

            Paciente PacienteTres = gerador.GerandoPaciente();

            var PacienteList = repositorioPaciente.SelecionarTodos();

            for (int i = 0; i < PacienteList.Count; i++)
            {
                Assert.AreEqual(Paciente.Nome, PacienteList[i].Nome);
            }
        }


    }
}
