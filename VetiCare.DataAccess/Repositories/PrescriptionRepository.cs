using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class PrescriptionRepository : GenericRepository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(VetiCareDbContext context) : base(context) { }
    }
}
