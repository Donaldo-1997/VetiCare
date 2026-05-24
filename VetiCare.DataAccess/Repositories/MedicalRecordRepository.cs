using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(VetiCareDbContext context) : base(context) { }
        
    }
}
