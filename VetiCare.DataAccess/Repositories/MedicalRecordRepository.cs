using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(VetiCareDbContext context) : base(context) { }

        public async Task<MedicalRecord?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(mr => mr.Pet)
                    .ThenInclude(p => p.Owner)
                .Include(mr => mr.Appointment)
                    .ThenInclude(a => a.Vet)
                .FirstOrDefaultAsync(mr => mr.Id == id);
        }

        public async Task<IEnumerable<MedicalRecord>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(mr => mr.Pet)
                    .ThenInclude(p => p.Owner)
                .Include(mr => mr.Appointment)
                    .ThenInclude(a => a.Vet)
                .ToListAsync();
        }
    }
}
