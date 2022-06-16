using ControleMedicamentos.Dominio.ModuloFornecedor;
using ControleMedicamentos.Dominio.ModuloFuncionario;
using ControleMedicamentos.Dominio.ModuloMedicamento;
using ControleMedicamentos.Dominio.ModuloRequisicao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ControleMedicamentos.Dominio.Tests.ModuloMedicamento
{
    [TestClass]
    public class FuncionarioTest
    {
        [TestMethod]
        public void Teste_Nome()
        {
            var medicamento = new Medicamento(null, "aaaa", "aaaaa", DateTime.MaxValue);

            ValidadorMedicamento validador = new ValidadorMedicamento();

            var validação = validador.Validate(medicamento);

            Assert.AreEqual("É Preciso preencher o campo Nome", validação.Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Teste_Descricao()
        {
            var medicamento = new Medicamento("bbbbb", null, "aaaaa", DateTime.MaxValue);

            ValidadorMedicamento validador = new ValidadorMedicamento();

            var validação = validador.Validate(medicamento);

            Assert.AreEqual("É Preciso preencher o campo Descricao", validação.Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Teste_Validade()
        {
            var medicamento = new Medicamento("bbbbb", "ddddd", "aaaaa", null);

            ValidadorMedicamento validador = new ValidadorMedicamento();

            var validação = validador.Validate(medicamento);

            Assert.AreEqual("É Preciso preencher o campo Validade", validação.Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Teste_Lote()
        {
            var medicamento = new Medicamento("bbbbb", "ddddd", null, DateTime.Now);

            ValidadorMedicamento validador = new ValidadorMedicamento();

            var validação = validador.Validate(medicamento);

            Assert.AreEqual("É Preciso preencher o campo Lote", validação.Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Deve_Adicionar_Requisicao()
        {
            var medicamento = new Medicamento("bbbbb", "ddddd", "cccccc", DateTime.Now);

            var requisicao = new Requisicao();

            requisicao.QtdMedicamento = 2;

            medicamento.Requisicoes.Add(requisicao);

            Assert.AreEqual(requisicao.QtdMedicamento, medicamento.Requisicoes[0].QtdMedicamento);
        }
        [TestMethod]
        public void Deve_Adicionar_Fornecedor()
        {
            var medicamento = new Medicamento("bbbbb", "ddddd", "cccccc", DateTime.Now);

            var fornecedor= new Fornecedor("aaaaa","aadddd","aaaaaaa","ddddddd","ccccccccccc");

            medicamento.Fornecedor = fornecedor;

            Assert.AreEqual(fornecedor, medicamento.Fornecedor);


        }
    }
}
