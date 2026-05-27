using FluentValidation;
using VetiCare.API.DTOs.Request;

namespace VetiCare.API.Validators
{
    public class AppointmentValidator : AbstractValidator<AppointmentRequestDTO>
    {
        public AppointmentValidator()
        {
            RuleFor(x => x.ScheduledAt)
                .NotEmpty().WithMessage("La fecha y hora de la cita son obligatorias.")
                .GreaterThan(DateTime.UtcNow).WithMessage("La cita debe agendarse en una fecha futura.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("El motivo de la cita es obligatorio.")
                .MaximumLength(500).WithMessage("El motivo no puede superar los 500 caracteres.");

            RuleFor(x => x.PetId)
                .GreaterThan(0).WithMessage("Debe indicar una mascota válida.");

            RuleFor(x => x.VetId)
                .GreaterThan(0).WithMessage("Debe indicar un veterinario válido.");
        }
    }
}
