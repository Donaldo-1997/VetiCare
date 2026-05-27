using VetiCare.Domain.Entities;
using VetiCare.Domain.Enums;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        /// <summary>Devuelve una cita con Pet, Vet y MedicalRecords cargados.</summary>
        Task<Appointment?> GetWithDetailsAsync(int id);

        /// <summary>Devuelve todas las citas de una mascota con detalles.</summary>
        Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId);

        /// <summary>Devuelve todas las citas asignadas a un veterinario.</summary>
        Task<IEnumerable<Appointment>> GetByVetIdAsync(int vetId);

        /// <summary>Devuelve todas las citas filtradas por estado.</summary>
        Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status);
    }
}
