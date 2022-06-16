using ControleMedicamentos.Dominio.ModuloPaciente;
using ControleMedicamentos.Infra.BancoDados.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloPaciente
{
    [TestClass]
    public class RepositorioPacienteEmBancoDadosTest
    {
        private string sql = @"DELETE FROM [TBPaciente]";
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
            var Repositorio = new RepositorioPacienteEmBancoDeDados();

            Paciente novoPaciente = new Paciente("Carlos", "1233344");

            Repositorio.Inserir(novoPaciente);

          var PacientePorNumero=  Repositorio.SelecionarPacienteNumero(novoPaciente.Id);

            Assert.IsNotNull(PacientePorNumero);
            Assert.AreEqual(novoPaciente.Id, PacientePorNumero.Id);
            Assert.AreEqual(novoPaciente.Nome, PacientePorNumero.Nome);
            Assert.AreEqual(novoPaciente.CartaoSUS, PacientePorNumero.CartaoSUS);
        }
        [TestMethod]
        public void Deve_Editar_Paciente()
        {
            var Repositorio = new RepositorioPacienteEmBancoDeDados();

            Paciente novoPaciente = new Paciente("Carlos", "1233344");

            Repositorio.Inserir(novoPaciente);

            var pacienteNovo = Repositorio.SelecionarPacienteNumero(novoPaciente.Id);

            pacienteNovo.Nome = "jOÃO";
            pacienteNovo.CartaoSUS = "223424242";

            Repositorio.Editar(pacienteNovo);

            var paciente = Repositorio.SelecionarPacienteNumero(pacienteNovo.Id);

            Assert.IsNotNull(paciente);
            Assert.AreEqual(paciente.Nome, pacienteNovo.Nome);
            Assert.AreEqual(paciente.CartaoSUS, pacienteNovo.CartaoSUS);

        }
        [TestMethod]
        public void Deve_Excluir_Paciente()
        {
            Paciente novoPaciente = new Paciente("Wandergol","242424242");


            var repositorio = new RepositorioPacienteEmBancoDeDados();

            repositorio.Excluir(novoPaciente);

            var PacienteExcluido = repositorio.SelecionarPacienteNumero(novoPaciente.Id);

            Assert.IsNull(PacienteExcluido);
        }
        [TestMethod]
        public void Deve_Selecionar_Todos()
        {
            Paciente novoPacienteUm = new Paciente("Carlos De Pena", "2431313131");
            Paciente novoPacienteDois = new Paciente("Alan Patrick", "2434441313131");
            Paciente novoPacienteTres = new Paciente("Pedro Henrique","76767676776");

            var repositorio = new RepositorioPacienteEmBancoDeDados();

            repositorio.Inserir(novoPacienteUm);
            repositorio.Inserir(novoPacienteDois);
            repositorio.Inserir(novoPacienteTres);

            List<Paciente> lista = repositorio.SelecionarTodos();

            Assert.AreEqual(3,lista.Count);

            Assert.AreEqual("Carlos De Pena", lista[0].Nome);
            Assert.AreEqual("Alan Patrick", lista[1].Nome);
            Assert.AreEqual("Pedro Henrique", lista[2].Nome);
        }


    }
}
