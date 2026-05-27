using FluentValidation;
using VetiCare.API.DTOs.Request;

namespace VetiCare.API.Validators
{
    public class VetValidator : AbstractValidator<VetRequestDTO>
    {
        public VetValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(100).WithMessage("El apellido no puede superar 100 caracteres.");

            RuleFor(x => x.LicenseNumber)
                .NotEmpty().WithMessage("El número de licencia es obligatorio.")
                .MaximumLength(50).WithMessage("El número de licencia no puede superar 50 caracteres.");

            RuleFor(x => x.Specialty)
                .NotEmpty().WithMessage("La especialidad es obligatoria.")
                .MaximumLength(150).WithMessage("La especialidad no puede superar 150 caracteres.");
        }
    }
}