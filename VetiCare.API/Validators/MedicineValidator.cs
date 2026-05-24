using FluentValidation;
using VetiCare.API.DTOs.Request;

namespace VetiCare.API.Validators
{
    public class MedicineValidator : AbstractValidator<MedicineRequestDTO>
    {
        public MedicineValidator() {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("El nombre de la medicina es obligatorio")
               .MaximumLength(500);
            RuleFor(x => x.ActiveIngredient)
                .NotEmpty().WithMessage("El ingrediente activo es obligatorio")
                .MaximumLength(500);
            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("La unidad de medida es obligatoria")
                .MaximumLength(90);


        }
    }
}
