using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(VetiCareDbContext context) : base(context) { }
        public async Task<Medicine> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(m => m.Name == name);
        }

    }
}
