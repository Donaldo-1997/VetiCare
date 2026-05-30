using FluentValidation;
using VetiCare.API.DTOs.Request;

namespace VetiCare.API.Validators
{
    public class BreedValidator : AbstractValidator<BreedRequestDTO>
    {
        public BreedValidator() { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la raza es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre de la raza no puede exceder los 100 caracteres.");
            RuleFor(x => x.Species)
                .NotEmpty().WithMessage("La especie de la raza es obligatoria.")
                .MaximumLength(100).WithMessage("La especie de la raza no puede exceder los 100 caracteres.");
        }
    }
}
