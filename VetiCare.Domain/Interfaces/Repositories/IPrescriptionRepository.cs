using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IPrescriptionRepository : IGenericRepository<Prescription>
    {
        Task<IEnumerable<Prescription>> GetbyMedicalRecordIdAsync(int medicalRecordId);
        Task<IEnumerable<Prescription>> GetbyMedicineIdAsync(int medicineId);
    }
}
