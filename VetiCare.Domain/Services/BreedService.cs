using Microsoft.Extensions.Logging;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;
using VetiCare.Domain.Interfaces.Services;

namespace VetiCare.Domain.Services
{
    public class BreedService : IBreedService
    {
        private readonly IBreedRepository _breedRepository;
        private readonly ILogger<BreedService> _logger;
        public BreedService(IBreedRepository breedRepository, ILogger<BreedService> logger)
        {
            _breedRepository = breedRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Breed>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all breeds");
            return await _breedRepository.GetAllAsync();
        }

        public async Task<Breed?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving breed with ID {Id}", id);
            return await _breedRepository.GetByIdAsync(id);
        }

        public async Task<Breed?> GetByNameAsync(string name)
        {
            _logger.LogInformation("Retrieving breed with name {Name}", name);
            return await _breedRepository.GetByNameAsync(name);
        }

        public async Task<IEnumerable<Breed>> GetBySpeciesAsync(string species)
        {
            _logger.LogInformation("Retrieving breeds with species {Species}", species);
            return await _breedRepository.GetBySpeciesAsync(species);
        }

       

        public async Task<Breed> CreateAsync(Breed breed)
        {
            var existingName = await _breedRepository.GetByNameAsync(breed.Name);
            if (existingName != null)
            {
                _logger.LogWarning("Breed with name {Name} already exists", breed.Name);
                throw new InvalidOperationException($"Ya existe una raza con el nombre {breed.Name}");
            }
            _logger.LogInformation("Creating new breed");
            return await _breedRepository.CreateAsync(breed);
        }
        
        public async Task UpdateAsync(int id, Breed breed)
        {
            var existingBreed = await GetByIdAsync(id);
            if (existingBreed == null)
            {
                _logger.LogWarning("Breed with ID {Id} not found for update", id);
                throw new KeyNotFoundException($"No se encontró una raza con el ID {id}");
            }
            existingBreed.Name = breed.Name;
            existingBreed.Species = breed.Species;
            _logger.LogInformation("Updating breed with ID {Id}", id);
            await _breedRepository.UpdateAsync(existingBreed);
           
        }
        public async Task DeleteAsync(int id)
        {
            var existingBreed = await GetByIdAsync(id);
            if (existingBreed == null)
            {
                _logger.LogWarning("Breed with ID {Id} not found for deletion", id);
                throw new KeyNotFoundException($"No se encontró una raza con el ID {id}");
            }
            _logger.LogInformation("Deleting breed with ID {Id}", id);
            await _breedRepository.DeleteAsync(id);


        }

        
    }
}
