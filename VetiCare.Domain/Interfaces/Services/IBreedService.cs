using VetiCare.Domain.Entities;

namespace VetiCare.Domain.Interfaces.Services
{
    public interface IBreedService
    {
        Task<IEnumerable<Breed>> GetAllAsync();
        Task<Breed?> GetByIdAsync(int id);
        Task<Breed?> GetByNameAsync(string name);
        Task<IEnumerable<Breed>> GetBySpeciesAsync(string species);
        Task<Breed> CreateAsync(Breed breed);
        Task UpdateAsync(int id, Breed breed);
        Task DeleteAsync(int id);
    }
}
