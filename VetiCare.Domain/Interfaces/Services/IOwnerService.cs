using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Services
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner?> GetByIdAsync(int id);
        Task<Owner?> GetByNameAsync(string name);
        Task<Owner?> GetByEmailAsync(string email);
        Task<Owner?> GetByPhoneAsync(string phone);
        Task<Owner?> GetByIdWithPetsAsync(int id);
        Task<IEnumerable<Owner>> GetAllWithPetsAsync();
        Task<Owner> CreateAsync( Owner owner);
        Task UpdateAsync(int id, Owner owner);
        Task DeleteAsync(int id);
    }
}
