using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Repositories
{
    public interface IBreedRepository : IGenericRepository<Breed>
    {
        Task<Breed?> GetByNameAsync(string name);
        Task<IEnumerable<Breed>> GetBySpeciesAsync(string species);
    }
}
