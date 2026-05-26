using FluentValidation;
using VetiCare.API.DTOs.Request;

namespace VetiCare.API.Validators
{
    public class MedicalRecordValidator : AbstractValidator<MedicalRecordRequestDTO>
    {
        public MedicalRecordValidator() {

            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("El diagnóstico es obligatorio")
                .MaximumLength(500);

            RuleFor(x => x.Treatment)
                .NotEmpty().WithMessage("El tratamiento es obligatorio")
                .MaximumLength(500);
            RuleFor(x => x.PetId)
                .GreaterThan(0).WithMessage("Debes indicar la mascota");


        }
    }
}
