using ControleMedicamentos.Dominio.ModuloPaciente;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamentos.Dominio.Tests.ModuloPaciente
{
    [TestClass]
    public class FuncionarioTest
    {
        [TestMethod]
        public void Nome_Deve_Ser_Obrigatorio()
        {            
            var PacienteTeste = new Paciente(null,"aaa");

            ValidadorPaciente validador = new ValidadorPaciente();
            var validação =  validador.Validate(PacienteTeste);

            Assert.AreEqual("É Preciso preencher o campo Nome", validação.Errors[0].ErrorMessage);
        }
        [TestMethod]
        public void Sus_Deve_Ser_Obrigatorio()
        {
            var PacienteTeste = new Paciente("aaaa",null);

            ValidadorPaciente validador = new ValidadorPaciente();
            var validação = validador.Validate(PacienteTeste);

            Assert.AreEqual("É Preciso preencher o campo Cartão Do Sus", validação.Errors[0].ErrorMessage);
        }
    }
}
