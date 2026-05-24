using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IMedicineRepository : IGenericRepository<Medicine>
    {
        Task<Medicine?> GetByNameAsync(string name);
    }
}
