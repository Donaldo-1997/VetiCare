using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(VetiCareDbContext context) : base(context) { }

        public async Task<Owner?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(o => o.FirstName == name || o.LastName == name);
        }

        public async Task<Owner?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(o => o.Email == email);
        }

        public async Task<Owner?> GetByPhoneAsync(string phone)
        {
            return await _dbSet
                .FirstOrDefaultAsync(o => o.Phone == phone);
        }
        public async Task<Owner?> GetByIdWithPetsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Pets)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<IEnumerable<Owner>> GetAllWithPetsAsync()
        {
            return await _dbSet
                .Include(o => o.Pets)
                .ToListAsync();
        }
    }
}
