using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloMedicamento
{
    
        public class ValidadorMedicamento : AbstractValidator<Medicamento>
        {
            public ValidadorMedicamento()
            {
                RuleFor(x => x.Nome)
                      .NotNull().WithMessage("É Preciso preencher o campo Nome");

                RuleFor(x => x.Validade)
                     .NotNull().WithMessage("É Preciso preencher o campo Validade");


                RuleFor(x => x.Descricao)
                     .NotNull().WithMessage("É Preciso preencher o campo Descricao");

                RuleFor(x => x.Lote)
                     .NotNull().WithMessage("É Preciso preencher o campo Lote");


        }
        }
}
