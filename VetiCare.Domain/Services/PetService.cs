using Microsoft.Extensions.Logging;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;
using VetiCare.Domain.Interfaces.Services;

namespace VetiCare.Domain.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly ILogger<PetService> _logger;
        public PetService(IPetRepository petRepository, ILogger<PetService> logger)
        {
            _petRepository = petRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Pet>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all pets");
            return await _petRepository.GetAllAsync();
        }
        public async Task<Pet?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving pet with ID {Id}", id);
            return await _petRepository.GetByIdAsync(id);
        }

        public async Task<Pet> CreateAsync(Pet pet)
        {
            _logger.LogInformation("Creating new pet");
            return await _petRepository.CreateAsync(pet);
        }

        public async Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId)
        {
            var ownerExists = await _petRepository.ExistsAsync(ownerId);
            if (!ownerExists)
            {
                _logger.LogWarning("Owner with ID {OwnerId} not found", ownerId);
                throw new KeyNotFoundException($"No se encontró el propietario con ID {ownerId}");
            }
            _logger.LogInformation("Retrieving pets for owner ID {OwnerId}", ownerId);
            return await _petRepository.GetByOwnerIdAsync(ownerId);
        }

        public async Task<Pet?> GetByIdWithDetailsAsync(int id)
        {
            _logger.LogInformation("Retrieving pet with details, ID {Id}", id);
            return await _petRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Pet>> GetAllWithDetailsAsync()
        {
            _logger.LogInformation("Retrieving all pets with details");
            return await _petRepository.GetAllWithDetailsAsync();
        }

        public async Task UpdateAsync(int id, Pet pet)
        {
            var existing = await _petRepository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Pet with ID {Id} not found for update", id);
                throw new KeyNotFoundException($"No se encontró la mascota con ID {id}");
            }
            existing.Name = pet.Name;
            existing.BirthDate = pet.BirthDate;
            existing.Weight = pet.Weight;
            existing.Gender = pet.Gender;
            existing.OwnerId = pet.OwnerId;
            existing.BreedId = pet.BreedId;
            _logger.LogInformation("Updating pet with ID {Id}", id);
            await _petRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _petRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Pet with ID {Id} not found for deletion", id);
                throw new KeyNotFoundException($"No se encontró la mascota con ID {id}");
            }
            _logger.LogInformation("Deleting pet with ID {Id}", id);
            await _petRepository.DeleteAsync(id);
        }
    }
}
