using ControleMedicamentos.Dominio.ModuloFuncionario;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleMedicamentos.Dominio.Tests.ModuloFuncionario
{
    [TestClass]
    public class FuncionarioTest
    {
        
        [TestMethod]                
        public void Nome_Deve_Ser_Obrigatório()            
        {
            var funcionarioTeste = new Funcionario();

            funcionarioTeste.Nome = null; 

            ValidadorFuncionario funcionario = new ValidadorFuncionario();

            var validação = funcionario.Validate(funcionarioTeste);

            Assert.AreEqual("É Preciso preencher o campo Nome",validação.Errors[0].ErrorMessage);
        }
        public void Login_Deve_Ser_Obrigatório()
        {
            var funcionarioTeste = new Funcionario();

            funcionarioTeste.Nome = "aaa";

            funcionarioTeste.Login = null;

            ValidadorFuncionario funcionario = new ValidadorFuncionario();

            var validação = funcionario.Validate(funcionarioTeste);

            Assert.AreEqual("É Preciso preencher o campo Login", validação.Errors[0].ErrorMessage);
        }
        public void Senha_Deve_Ser_Obrigatório()
        {
            var funcionarioTeste = new Funcionario();

            funcionarioTeste.Nome = "aaa";                    

            funcionarioTeste.Login = "ddddd";

            funcionarioTeste.Senha= null;            

            ValidadorFuncionario funcionario = new ValidadorFuncionario();

            var validação = funcionario.Validate(funcionarioTeste);

            Assert.AreEqual("É Preciso preencher o campo Senha", validação.Errors[0].ErrorMessage);
        }



    }
}
