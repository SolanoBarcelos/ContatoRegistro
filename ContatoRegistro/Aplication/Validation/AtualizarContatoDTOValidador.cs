using ContatoRegistro.Aplication.DTOs;
using FluentValidation;

namespace ContatoRegistro.Aplication.Validation
{
    public sealed class AtualizarContatoDtoValidator : AbstractValidator<AtualizarContatoDTO>
    {
        public AtualizarContatoDtoValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MinimumLength(2);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Telefone).NotEmpty().Matches(@"^\D*(\d\D*){10,11}$");
        }
    }
}
