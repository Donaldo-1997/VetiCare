using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IPetRepository : IGenericRepository<Pet>
    {
        Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId);
        Task<Pet?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Pet>> GetAllWithDetailsAsync();
    }
}
