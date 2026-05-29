using FluentValidation;
using VetiCare.API.DTOs.Request;

namespace VetiCare.API.Validators
{
    public class OwnerValidator : AbstractValidator<OwnerRequestDTO>
    {
        public OwnerValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El email no tiene formato válido");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("El teléfono es obligatorio")
                .MaximumLength(20);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("La dirección es obligatoria")
                .MaximumLength(200);
        }
    }
}
