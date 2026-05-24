using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Services
{
    public interface IMedicineService
    {
        Task<IEnumerable<Medicine>> GetAllAsync();
        Task<Medicine?> GetByIdAsync(int id);
        Task<Medicine> GetByNameAsync(string name);
        Task<Medicine> CreateAsync(Medicine medicine);
        Task UpdateAsync(int id, Medicine medicine);
        Task DeleteAsync(int id);
    }
}
