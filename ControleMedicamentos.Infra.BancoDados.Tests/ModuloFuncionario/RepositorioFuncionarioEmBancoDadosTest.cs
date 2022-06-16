using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        private string sql = @"DELETE FROM [TBFuncionario]";
        private string EndereçoBanco = @"Data Source = (LocalDB)\MSSQLLocalDB;Initial Catalog = ControleDeMedicamentosDb; Integrated Security = True; Pooling=False";
        public RepositorioFuncionarioEmBancoDadosTest()
        {
            SqlConnection conexaoComBanco = new SqlConnection(EndereçoBanco);

            SqlCommand comando = new SqlCommand(sql, conexaoComBanco);

            conexaoComBanco.Open();

            comando.ExecuteNonQuery();

            conexaoComBanco.Close();
        }

        [TestMethod]

        public void Deve_inserir_Funcionario()
        {
            Funcionario novoFuncionario = new Funcionario();
            novoFuncionario.Login = "aaa";
            novoFuncionario.Senha = "BBB";
            novoFuncionario.Nome = "DDDD";


            var repositorio = new RepositorioFuncionarioEmBancoDeDados();

            //action
            repositorio.Inserir(novoFuncionario);

            //assert
            Funcionario FuncionarioPorNumero = repositorio.SelecionarFuncionarioPorNumero(novoFuncionario.Id);

            Assert.IsNotNull(FuncionarioPorNumero);
            Assert.AreEqual(novoFuncionario.Id, FuncionarioPorNumero.Id);
            Assert.AreEqual(novoFuncionario.Nome, FuncionarioPorNumero.Nome);
            Assert.AreEqual(novoFuncionario.Login, FuncionarioPorNumero.Login);
            Assert.AreEqual(novoFuncionario.Senha, FuncionarioPorNumero.Senha);
   
        }
        [TestMethod]
        public void Deve_Editar_Funcionario()
        {
            Funcionario novoFuncionario = new Funcionario();
            novoFuncionario.Login = "aaa";
            novoFuncionario.Senha = "BBB";
            novoFuncionario.Nome = "DDDD";

            var repositorio = new RepositorioFuncionarioEmBancoDeDados();
        
            repositorio.Inserir(novoFuncionario);

            Funcionario FuncionarioPorNumero = repositorio.SelecionarFuncionarioPorNumero(novoFuncionario.Id);

            FuncionarioPorNumero.Senha = "aaaaaa";
            FuncionarioPorNumero.Login = "ddddd";
            FuncionarioPorNumero.Nome = "DDDD";

            repositorio.Editar(FuncionarioPorNumero);

            Funcionario funcionarioEditado = repositorio.SelecionarFuncionarioPorNumero(novoFuncionario.Id);

            Assert.IsNotNull(FuncionarioPorNumero);
            Assert.AreEqual(FuncionarioPorNumero.Id, funcionarioEditado.Id);
            Assert.AreEqual(FuncionarioPorNumero.Nome, funcionarioEditado.Nome);
            Assert.AreEqual(FuncionarioPorNumero.Login, funcionarioEditado.Login);
            Assert.AreEqual(FuncionarioPorNumero.Senha, funcionarioEditado.Senha);
        }
        [TestMethod]
        public void Deve_Excluir_Funcionario()
        {
            Funcionario novoFuncionario = new Funcionario();
            novoFuncionario.Login = "aaa";
            novoFuncionario.Senha = "BBB";
            novoFuncionario.Nome = "DDDD";


            var repositorio = new RepositorioFuncionarioEmBancoDeDados();

            repositorio.Excluir(novoFuncionario);

            var FuncionarioExcluido = repositorio.SelecionarFuncionarioPorNumero(novoFuncionario.Id);

            Assert.IsNull(FuncionarioExcluido);
        }
        [TestMethod]
        public void Deve_Verificar_Todos_Funcionario()
        {
            Funcionario novoFuncionario = new Funcionario();
            novoFuncionario.Login = "aaa";
            novoFuncionario.Senha = "BBB";
            novoFuncionario.Nome = "DDDD";

            Funcionario novoFuncionarioDois = new Funcionario();
            novoFuncionarioDois.Login = "aaa";
            novoFuncionarioDois.Senha = "BBB";
            novoFuncionarioDois.Nome = "VVVV";

            Funcionario novoFuncionarioTres = new Funcionario();
            novoFuncionarioTres.Login = "aaa";
            novoFuncionarioTres.Senha = "BBB";
            novoFuncionarioTres.Nome = "AAAA";

            var repositorio = new RepositorioFuncionarioEmBancoDeDados();

            repositorio.Inserir(novoFuncionario);
            repositorio.Inserir(novoFuncionarioDois);
            repositorio.Inserir(novoFuncionarioTres);

            List<Funcionario> funcionarios = repositorio.SelecionarTodos();

            Assert.AreEqual(3, funcionarios.Count);

            Assert.AreEqual("DDDD", funcionarios[0].Nome);
            Assert.AreEqual("VVVV", funcionarios[1].Nome);
            Assert.AreEqual("AAAA", funcionarios[2].Nome);
        }
    }
}
