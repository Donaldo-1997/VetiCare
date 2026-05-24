using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Services
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecord>> GetAllAsync();
        Task<MedicalRecord?> GetByIdAsync(int id);
        Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord);
        Task UpdateAsync(int id, MedicalRecord medicalRecord);
        Task DeleteAsync(int id);

    }
}
