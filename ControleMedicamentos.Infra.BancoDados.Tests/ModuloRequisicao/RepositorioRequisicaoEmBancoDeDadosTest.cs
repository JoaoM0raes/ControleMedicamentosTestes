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

        }
        [TestMethod]
        public void Deve_Inserir_Requisicao()
        {
            var requisicao = new Requisicao();

            var repositorioRequisicao = new RepositorioRequisicaoEmBancoDeDados();

            var medicamento = new Medicamento("Amonixilina","Dor","1313131", DateTime.Today);
            var repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();
            

            Fornecedor fornecedor = new Fornecedor("AAA", "2424242", "AAA@GMAIL.COM", "LAGES", "SC");
            var repositorioFornecedor = new RepositorioFornecedorEmBancoDeDados();
            repositorioFornecedor.Inserir(fornecedor);

            repositorioMedicamento.Inserir(medicamento, fornecedor);

            var medicamentoNumero =repositorioMedicamento.SelecionarMedicamentoNumero(medicamento.Id);


            var funcionario = new Funcionario();
            funcionario.Nome = "Edenilson";
            funcionario.Senha = "aaa1313";
            funcionario.Login = "aaaaaaaaa";
            var repositorioFuncionario = new RepositorioFuncionarioEmBancoDeDados();           

            repositorioFuncionario.Inserir(funcionario);

            var funcionarioNumero = repositorioFuncionario.SelecionarFuncionarioPorNumero(funcionario.Id);

            var paciente = new Paciente("Carlos","aadawdadadad");
            var repositorioPaciente = new RepositorioPacienteEmBancoDeDados();

            repositorioPaciente.Inserir(paciente);

            var pacienteNumero= repositorioPaciente.SelecionarPacienteNumero(paciente.Id);

            requisicao.Paciente = pacienteNumero;
            requisicao.Medicamento = medicamentoNumero;
            requisicao.Funcionario = funcionarioNumero;
            requisicao.Data = DateTime.Today;
            requisicao.QtdMedicamento = 2;

            repositorioRequisicao.Inserir( requisicao);

            requisicao.Paciente= pacienteNumero;

            var requisicaoNova = repositorioRequisicao.SelecionarRequisicaoNumero(requisicao.Id);

            Assert.AreEqual(requisicaoNova.Id, requisicao.Id);
            Assert.AreEqual(requisicaoNova.Paciente.Nome, requisicao.Paciente.Nome);

        }
        [TestMethod]
        public void Deve_Editar_Requisicao()
        {
            var requisicao = new Requisicao();

            var repositorioRequisicao = new RepositorioRequisicaoEmBancoDeDados();

            var medicamento = new Medicamento("Amonixilina", "Dor", "1313131", DateTime.Today);
            var repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();


            Fornecedor fornecedor = new Fornecedor("AAA", "2424242", "AAA@GMAIL.COM", "LAGES", "SC");
            var repositorioFornecedor = new RepositorioFornecedorEmBancoDeDados();
            repositorioFornecedor.Inserir(fornecedor);

            repositorioMedicamento.Inserir(medicamento, fornecedor);

            var medicamentoNumero = repositorioMedicamento.SelecionarMedicamentoNumero(medicamento.Id);


            var funcionario = new Funcionario();
            funcionario.Nome = "Edenilson";
            funcionario.Senha = "aaa1313";
            funcionario.Login = "aaaaaaaaa";
            var repositorioFuncionario = new RepositorioFuncionarioEmBancoDeDados();

            repositorioFuncionario.Inserir(funcionario);

            var funcionarioNumero = repositorioFuncionario.SelecionarFuncionarioPorNumero(funcionario.Id);

            var paciente = new Paciente("Carlos", "aadawdadadad");
            var repositorioPaciente = new RepositorioPacienteEmBancoDeDados();

            repositorioPaciente.Inserir(paciente);

            var pacienteNumero = repositorioPaciente.SelecionarPacienteNumero(paciente.Id);

            requisicao.Paciente=pacienteNumero;
            requisicao.Medicamento=medicamentoNumero;
            requisicao.Funcionario = funcionarioNumero;
            requisicao.Data = DateTime.Today;
            requisicao.QtdMedicamento = 2;

            repositorioRequisicao.Inserir(requisicao);

            var requisicaoNova = repositorioRequisicao.SelecionarRequisicaoNumero(requisicao.Id);

            var pacienteDois = new Paciente("Carlos", "555555555");
            repositorioPaciente.Inserir(pacienteDois);
            var pacienteNumeroDois = repositorioPaciente.SelecionarPacienteNumero(paciente.Id);

            requisicaoNova.QtdMedicamento = 20; 
            requisicaoNova.Paciente = pacienteDois;

            repositorioRequisicao.Editar(requisicaoNova);

           var requisicaoEditada = repositorioRequisicao.SelecionarRequisicaoNumero(requisicaoNova.Id);

            Assert.AreEqual(requisicaoEditada.QtdMedicamento,requisicaoNova.QtdMedicamento);
            Assert.AreEqual(requisicaoEditada.Paciente.CartaoSUS,requisicaoNova.Paciente.CartaoSUS);
        }
        [TestMethod]
        public void Deve_Excluir()
        {
            var requisicao = new Requisicao();

            var repositorioRequisicao = new RepositorioRequisicaoEmBancoDeDados();

            var medicamento = new Medicamento("Amonixilina", "Dor", "1313131", DateTime.Today);
            var repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();


            Fornecedor fornecedor = new Fornecedor("AAA", "2424242", "AAA@GMAIL.COM", "LAGES", "SC");
            var repositorioFornecedor = new RepositorioFornecedorEmBancoDeDados();
            repositorioFornecedor.Inserir(fornecedor);

            repositorioMedicamento.Inserir(medicamento, fornecedor);

            var medicamentoNumero = repositorioMedicamento.SelecionarMedicamentoNumero(medicamento.Id);


            var funcionario = new Funcionario();
            funcionario.Nome = "Edenilson";
            funcionario.Senha = "aaa1313";
            funcionario.Login = "aaaaaaaaa";
            var repositorioFuncionario = new RepositorioFuncionarioEmBancoDeDados();

            repositorioFuncionario.Inserir(funcionario);

            var funcionarioNumero = repositorioFuncionario.SelecionarFuncionarioPorNumero(funcionario.Id);

            var paciente = new Paciente("Carlos", "aadawdadadad");
            var repositorioPaciente = new RepositorioPacienteEmBancoDeDados();

            repositorioPaciente.Inserir(paciente);

            var pacienteNumero = repositorioPaciente.SelecionarPacienteNumero(paciente.Id);

            requisicao.Paciente = pacienteNumero;
            requisicao.Medicamento = medicamentoNumero;
            requisicao.Funcionario = funcionarioNumero;
            requisicao.Data = DateTime.Today;
            requisicao.QtdMedicamento = 2;

            repositorioRequisicao.Excluir(requisicao);

            var requisicaoExcluida =repositorioRequisicao.SelecionarRequisicaoNumero(requisicao.Id);

            Assert.IsNull(requisicaoExcluida.Funcionario);
        }

        [TestMethod]
        public void Deve_Selecionar_Todos()
        {

            var requisicao = new Requisicao();

            var requisicaoDois  = new Requisicao();

            var requisicaoTres = new Requisicao();

            var repositorioRequisicao = new RepositorioRequisicaoEmBancoDeDados();

            var medicamento = new Medicamento("Amonixilina", "Dor", "1313131", DateTime.Today);
            var repositorioMedicamento = new RepositorioMedicamentoEmBancoDados();


            Fornecedor fornecedor = new Fornecedor("AAA", "2424242", "AAA@GMAIL.COM", "LAGES", "SC");
            var repositorioFornecedor = new RepositorioFornecedorEmBancoDeDados();
            repositorioFornecedor.Inserir(fornecedor);

            repositorioMedicamento.Inserir(medicamento, fornecedor);

            var medicamentoNumero = repositorioMedicamento.SelecionarMedicamentoNumero(medicamento.Id);


            var funcionario = new Funcionario();
            funcionario.Nome = "Edenilson";
            funcionario.Senha = "aaa1313";
            funcionario.Login = "aaaaaaaaa";
            var repositorioFuncionario = new RepositorioFuncionarioEmBancoDeDados();

            repositorioFuncionario.Inserir(funcionario);

            var funcionarioNumero = repositorioFuncionario.SelecionarFuncionarioPorNumero(funcionario.Id);

            var paciente = new Paciente("Carlos", "aadawdadadad");
            var repositorioPaciente = new RepositorioPacienteEmBancoDeDados();

            repositorioPaciente.Inserir(paciente);

            var pacienteNumero = repositorioPaciente.SelecionarPacienteNumero(paciente.Id);

            requisicao.Paciente = pacienteNumero;
            requisicao.Medicamento = medicamentoNumero;
            requisicao.Funcionario = funcionarioNumero;
            requisicao.Data = DateTime.Today;
            requisicao.QtdMedicamento = 2;

            requisicaoDois.Paciente = pacienteNumero;
            requisicaoDois.Medicamento = medicamentoNumero;
            requisicaoDois.Funcionario = funcionarioNumero;
            requisicaoDois.Data = DateTime.Today;
            requisicaoDois.QtdMedicamento = 20;

            requisicaoTres.Paciente=pacienteNumero; 
            requisicaoTres.Data = DateTime.Today;
            requisicaoTres.Funcionario= funcionarioNumero;
            requisicaoTres.QtdMedicamento = 30;
            requisicaoTres.Medicamento= medicamentoNumero;

            repositorioRequisicao.Inserir(requisicao);
            repositorioRequisicao.Inserir(requisicaoDois);
            repositorioRequisicao.Inserir(requisicaoTres);

            var listaRequisicao = repositorioRequisicao.SelecionarTodos();

            Assert.AreEqual(requisicao.QtdMedicamento, listaRequisicao[0].QtdMedicamento);
            Assert.AreEqual(requisicaoDois.QtdMedicamento, listaRequisicao[1].QtdMedicamento);
            Assert.AreEqual(requisicaoTres.QtdMedicamento, listaRequisicao[2].QtdMedicamento);
           

            
            
            
        }

    }
}
