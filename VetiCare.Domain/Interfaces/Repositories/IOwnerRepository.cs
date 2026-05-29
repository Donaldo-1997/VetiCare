using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
      
        Task<Owner?> GetByEmailAsync(string email);
        Task<Owner?> GetByPhoneAsync(string phone);
        Task<Owner?> GetByIdWithPetsAsync(int id);
        Task<IEnumerable<Owner>> GetAllWithPetsAsync();
        Task<Owner?> GetByNameAsync(string name);
        
    }
}
