using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Services
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<Prescription>> GetAllAsync();
        Task<Prescription?> GetByIdAsync(int id);
        Task<Prescription> CreateAsync(Prescription prescription);
        Task<IEnumerable<Prescription>> GetByMedicalRecordAsync(int medicalRecordId);
        Task<IEnumerable<Prescription>> GetByMedicineAsync(int medicineId);
        Task<Prescription?> GetByIdWithDetailsAsync(int id);
        Task UpdateAsync(int id, Prescription prescription);
        Task DeleteAsync(int id);
        
       
    }
}
