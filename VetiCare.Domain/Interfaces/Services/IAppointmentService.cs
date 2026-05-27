using VetiCare.Domain.Entities;
using VetiCare.Domain.Enums;

namespace VetiCare.Domain.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<Appointment?> GetWithDetailsAsync(int id);
        Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId);
        Task<IEnumerable<Appointment>> GetByVetIdAsync(int vetId);
        Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status);
        Task<Appointment> CreateAsync(Appointment appointment);
        Task UpdateAsync(int id, Appointment appointment);
        Task UpdateStatusAsync(int id, AppointmentStatus newStatus);
        Task DeleteAsync(int id);
    }
}
