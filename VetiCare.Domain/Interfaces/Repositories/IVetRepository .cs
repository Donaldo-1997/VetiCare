using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IVetRepository : IGenericRepository<Vet>
    {
        Task<IEnumerable<Vet>> GetAllVetsWithAppointmentsAsync();

    }
}
