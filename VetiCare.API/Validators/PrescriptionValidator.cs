using FluentValidation;
using VetiCare.API.DTOs.Request;

namespace VetiCare.API.Validators
{
    public class PrescriptionValidator : AbstractValidator<PrescriptionRequestDTO>
    {
        public PrescriptionValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("La cantidad debe ser mayor a cero");
            RuleFor(x => x.Dosage)
                .NotEmpty().WithMessage("La dosis es obligatoria")
                .MaximumLength(500);
            RuleFor(x => x.Instructions)
                .NotEmpty().WithMessage("Las instrucciones son obligatorias")
                .MaximumLength(500);
            RuleFor(x => x.MedicalRecordId)
                .GreaterThan(0).WithMessage("Debes indicar el historial médico");
            RuleFor(x => x.MedicineId)
                .GreaterThan(0).WithMessage("Debes indicar la medicina");
        }
    }
}
