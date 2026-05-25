using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(VetiCareDbContext context) : base(context) { }


        public async Task<IEnumerable<Prescription>> GetbyMedicalRecordIdAsync(int medicalRecordId)
        {
            return await _dbSet
                .Where(p => p.MedicalRecordId == medicalRecordId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetbyMedicineIdAsync(int medicineId)
        {
            return await _dbSet
                .Where(p => p.MedicineId == medicineId)
                .ToListAsync();
        }

        public async Task<Prescription?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.MedicalRecord)
                .Include(p => p.Medicine)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
