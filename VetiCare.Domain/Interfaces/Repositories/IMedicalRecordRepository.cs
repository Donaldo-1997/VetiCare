using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        
        Task<MedicalRecord?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<MedicalRecord>> GetAllWithDetailsAsync();
    }
}
