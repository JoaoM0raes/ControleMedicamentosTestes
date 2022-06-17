using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Infra.BancoDados.ModuloFuncionario;
using GeradorClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ControleMedicamento.Infra.BancoDados.Tests.ModuloFuncionario
{
    [TestClass]
    public class RepositorioFuncionarioEmBancoDadosTest
    {
        RepositorioFuncionarioEmBancoDeDados repositorio = new RepositorioFuncionarioEmBancoDeDados();

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
            Funcionario funcionario = gerador.GerarandoFuncionario();
            Assert.IsNotNull(funcionario);
   
        }
        [TestMethod]
        public void Deve_Editar_Funcionario()
        {
            Funcionario funcionario=gerador.GerarandoFuncionario();

            funcionario.Nome = "aaaaaaaaa";

            repositorio.Editar(funcionario);

            var funcionarioEditado = repositorio.SelecionarFuncionarioPorNumero(funcionario.Id);

            Assert.AreEqual(funcionario.Nome, funcionarioEditado.Nome);
        }
        [TestMethod]
        public void Deve_Excluir_Funcionario()
        {
            Funcionario funcionario = gerador.GerarandoFuncionario();

            repositorio.Excluir(funcionario);

            var funcionarioExcluido = repositorio.SelecionarFuncionarioPorNumero(funcionario.Id);

            Assert.IsNull(funcionarioExcluido);


        }
        [TestMethod]
        public void Deve_Verificar_Todos_Funcionario()
        {
            Funcionario funcionarioUm = gerador.GerarandoFuncionario();
            Funcionario funcionarioDois = gerador.GerarandoFuncionario();
            Funcionario funcionarioTres = gerador.GerarandoFuncionario();

            var listaFuncionario = repositorio.SelecionarTodos();

            for (int i = 0; i < listaFuncionario.Count; i++)
            {
                Assert.AreEqual(funcionarioUm.Nome,listaFuncionario[i].Nome);
            }
        }
    }
}
