using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IPrescriptionRepository : IGenericRepository<Prescription>
    {
        Task<Prescription?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Prescription>> GetbyMedicalRecordIdAsync(int medicalRecordId);
        Task<IEnumerable<Prescription>> GetbyMedicineIdAsync(int medicineId);
    }
}
