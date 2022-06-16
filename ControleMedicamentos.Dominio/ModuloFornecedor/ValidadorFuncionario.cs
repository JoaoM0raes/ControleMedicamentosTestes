using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Dominio.ModuloFornecedor
{
    public class ValidadorFornecedor : AbstractValidator<Fornecedor>
    {

        public ValidadorFornecedor()
        {
            RuleFor(x => x.Nome)
                    .NotNull().WithMessage("É Preciso preencher o campo Nome");

            RuleFor(x => x.Email)
                     .NotNull().WithMessage("É Preciso preencher o campo Email");

            RuleFor(x => x.Estado)
                     .NotNull().WithMessage("É Preciso preencher o campo Estado");

            RuleFor(x => x.Cidade)
                    .NotNull().WithMessage("É Preciso preencher o campo Cidade");

            RuleFor(x => x.Telefone)
                    .NotNull().WithMessage("É Preciso preencher o campo Telefone");
        }
        
    }
}
