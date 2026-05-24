using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
    {
        public MedicineRepository(VetiCareDbContext context) : base(context) { }
    }
}
