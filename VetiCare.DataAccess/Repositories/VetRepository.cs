using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class VetRepository : GenericRepository<Vet>, IVetRepository
    {
        public VetRepository(VetiCareDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Vet>> GetAllVetsWithAppointmentsAsync()
        {
            return await _context.Vets
                .Include(v => v.Appointments)
                .ToListAsync();
        }
    }
}
