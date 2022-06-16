namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class Funcionario : EntidadeBase<Funcionario>
    {

        public Funcionario()
        {
           
           
        }
        
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
