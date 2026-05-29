using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Services
{
    public interface IPetService
    {
        Task<IEnumerable<Pet>> GetAllAsync();
        Task<Pet?> GetByIdAsync(int id);
        Task<Pet> CreateAsync(Pet pet);
        Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId);
        Task<Pet?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Pet>> GetAllWithDetailsAsync();
        Task UpdateAsync(int id, Pet pet);
        Task DeleteAsync(int id);
        
    }
}
