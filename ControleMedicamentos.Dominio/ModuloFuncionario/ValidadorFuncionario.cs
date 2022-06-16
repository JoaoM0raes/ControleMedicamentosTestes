using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloFuncionario
{
    public class ValidadorFuncionario : AbstractValidator<Funcionario>
    {
        public ValidadorFuncionario()
        {
            RuleFor(x => x.Nome)
                  .NotNull().WithMessage("É Preciso preencher o campo Nome");

            RuleFor(x => x.Login)
                 .NotNull().WithMessage("É Preciso preencher o campo Login");


            RuleFor(x => x.Senha)
                 .NotNull().WithMessage("É Preciso preencher o campo Senha");
              
        }
    }
}
