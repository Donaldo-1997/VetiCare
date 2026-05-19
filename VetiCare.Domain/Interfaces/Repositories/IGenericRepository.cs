using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : AuditBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
