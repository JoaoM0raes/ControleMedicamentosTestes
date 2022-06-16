using ControleMedicamentos.Dominio.ModuloFuncionario;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloPaciente
{
    public class ValidadorPaciente : AbstractValidator<Paciente>
    {
        public ValidadorPaciente()
        {
            RuleFor(x => x.Nome)
                 .NotNull().WithMessage("É Preciso preencher o campo Nome");

            RuleFor(x => x.CartaoSUS)
                 .NotNull().WithMessage("É Preciso preencher o campo Cartão Do Sus");            
        }


    }
}
