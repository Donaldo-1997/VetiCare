using FluentValidation;
using VetiCare.API.DTOs.Request;

namespace VetiCare.API.Validators
{
    public class PetValidator : AbstractValidator<PetRequestDTO>
    {
        public PetValidator() { 
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El nombre de la mascota es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre de la mascota no puede exceder los 100 caracteres.");
            RuleFor(p => p.BreedId)
                .GreaterThan(0).WithMessage("El ID de la raza debe ser un número positivo.");
            RuleFor(p => p.BirthDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de nacimiento de la mascota no puede ser en el futuro.");
            RuleFor(p => p.Weight)
                .GreaterThan(0).WithMessage("El peso de la mascota debe ser un número positivo.");
            RuleFor(p => p.Gender)
                .IsInEnum().WithMessage("El género de la mascota no es válido.");

        }
    }
}
