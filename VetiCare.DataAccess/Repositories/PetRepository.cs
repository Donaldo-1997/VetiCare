using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        public PetRepository(VetiCareDbContext context) : base(context) { }
        public async Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId)
        {
            return await _dbSet
                .Where(p => p.OwnerId == ownerId)
                .ToListAsync();
        }
        public async Task<Pet?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Breed)
                .Include(p => p.Owner)
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Pet>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(p => p.Breed)
                .Include(p => p.Owner)
                .Include(p => p.MedicalRecords)
                .ToListAsync();
        }
    }
}
