using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class BreedRepository : GenericRepository<Breed>, IBreedRepository
    {
        public BreedRepository(VetiCareDbContext context) : base(context) { }

        public async Task<Breed?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.Name == name);
        }
        public async Task<Breed?> GetByIdWithPetsAsync(int id)
        {
            return await _dbSet
                .Include(b => b.Pets)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<IEnumerable<Breed>> GetBySpeciesAsync(string species)
        {
            return await _dbSet
                .Where(b => b.Species == species)
                .ToListAsync();
        }
    }
}
