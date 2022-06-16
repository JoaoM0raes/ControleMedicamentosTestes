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
using System;

namespace GeradorClasses
{
    public class GeradorDeClasses
    {
        RepositorioFuncionarioEmBancoDeDados repositorioFuncionario;
        RepositorioPacienteEmBancoDeDados repositorioPaciente;
        RepositorioFornecedorEmBancoDeDados repositorioFornecedor;
        RepositorioMedicamentoEmBancoDados repositorioMedicamento;
        RepositorioRequisicaoEmBancoDeDados repositorioRequisicao;

       public GeradorDeClasses()
        {
            this.repositorioFornecedor=new RepositorioFornecedorEmBancoDeDados();
            this.repositorioFuncionario=new RepositorioFuncionarioEmBancoDeDados();
            this.repositorioMedicamento=new RepositorioMedicamentoEmBancoDados();
            this.repositorioPaciente=new RepositorioPacienteEmBancoDeDados();
            this.repositorioRequisicao = new RepositorioRequisicaoEmBancoDeDados();
        }

        public Funcionario GerarandoFuncionario()
        {
            Funcionario funcionario = new Funcionario();
            funcionario.Login="joaomoraes";
            funcionario.Senha = "131231231";
            funcionario.Nome = "Joao";

            repositorioFuncionario.Inserir(funcionario);

            return repositorioFuncionario.SelecionarFuncionarioPorNumero(funcionario.Id);
        }
        public Paciente GerandoPaciente()
        {
            Paciente paciente = new Paciente("João Moraes","2424242");

            repositorioPaciente.Inserir(paciente);

            return repositorioPaciente.SelecionarPacienteNumero(paciente.Id);
        }

        public Fornecedor GerandoFornecedor()
        {
            Fornecedor fornecedor = new Fornecedor("João Moraes", "2232323232", "joao@gmail.com", "lages", "Sc");

            repositorioFornecedor.Inserir(fornecedor);

            return repositorioFornecedor.SelecionarFornecedorPorNumero(fornecedor.Id);
        }

        public Medicamento GerandoMedicamento()
        {
            Medicamento medicamento = new Medicamento("Amoxilina","Para dor","313131",DateTime.Today);

            Fornecedor fornecedor = GerandoFornecedor();

            repositorioMedicamento.Inserir(medicamento,fornecedor);

            return repositorioMedicamento.SelecionarMedicamentoNumero(medicamento.Id);
        }

        public Requisicao GerandoRequisicao()
        {
            Requisicao requisicao = new Requisicao();

            AtribuindoValoresRequisicao(requisicao);

            repositorioRequisicao.Inserir(requisicao);

            return repositorioRequisicao.SelecionarRequisicaoNumero(requisicao.Id);
        }

        private void AtribuindoValoresRequisicao(Requisicao requisicao)
        {
            requisicao.Paciente = GerandoPaciente();
            requisicao.Medicamento = GerandoMedicamento();
            requisicao.Funcionario = GerarandoFuncionario();
            requisicao.Data = DateTime.Today;
            requisicao.QtdMedicamento = 20;
        }



    }
}
