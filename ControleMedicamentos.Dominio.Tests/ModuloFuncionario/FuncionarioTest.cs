using ControleMedicamentos.Dominio.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamentos.Dominio.Tests.ModuloFuncionario
{
    [TestClass]
    public class FuncionarioTest
    {
        
        [TestMethod]                
        public void Nome_Deve_Ser_Obrigat�rio()            
        {
            var funcionarioTeste = new Funcionario();

            funcionarioTeste.Nome = null; 

            ValidadorFuncionario funcionario = new ValidadorFuncionario();

            var valida��o = funcionario.Validate(funcionarioTeste);

            Assert.AreEqual("� Preciso preencher o campo Nome",valida��o.Errors[0].ErrorMessage);
        }
        public void Login_Deve_Ser_Obrigat�rio()
        {
            var funcionarioTeste = new Funcionario();

            funcionarioTeste.Nome = "aaa";

            funcionarioTeste.Login = null;

            ValidadorFuncionario funcionario = new ValidadorFuncionario();

            var valida��o = funcionario.Validate(funcionarioTeste);

            Assert.AreEqual("� Preciso preencher o campo Login", valida��o.Errors[0].ErrorMessage);
        }
        public void Senha_Deve_Ser_Obrigat�rio()
        {
            var funcionarioTeste = new Funcionario();

            funcionarioTeste.Nome = "aaa";                    

            funcionarioTeste.Login = "ddddd";

            funcionarioTeste.Senha= null;            

            ValidadorFuncionario funcionario = new ValidadorFuncionario();

            var valida��o = funcionario.Validate(funcionarioTeste);

            Assert.AreEqual("� Preciso preencher o campo Senha", valida��o.Errors[0].ErrorMessage);
        }



    }
}
