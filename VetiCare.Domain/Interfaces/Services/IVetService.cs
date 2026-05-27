using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Services
{
    public interface IVetService
    {
        Task<IEnumerable<Vet>> GetAllAsync();
        Task<Vet?> GetByIdAsync(int id);
        Task<IEnumerable<Vet>> GetAllWithAppointmentsAsync();
        Task<Vet> CreateAsync(Vet vet);
        Task UpdateAsync(int id, Vet vet);
        Task DeleteAsync(int id);
    }
}
