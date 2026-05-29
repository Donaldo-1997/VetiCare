using Microsoft.Extensions.Logging;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;
using VetiCare.Domain.Interfaces.Services;

namespace VetiCare.Domain.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ILogger<OwnerService> _logger;
        public OwnerService(IOwnerRepository ownerRepository, ILogger<OwnerService> logger)
        {
            _ownerRepository = ownerRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all owners");
            return await _ownerRepository.GetAllAsync();
        }
        public async Task<Owner?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving owner with ID {Id}", id);
            return await _ownerRepository.GetByIdAsync(id);
        }
        public async Task<Owner?> GetByNameAsync(string name)
        {
            _logger.LogInformation("Retrieving owner with name {Name}", name);
            return await _ownerRepository.GetByNameAsync(name);
        }
        public async Task<Owner?> GetByEmailAsync(string email)
        {
            _logger.LogInformation("Retrieving owner with email {Email}", email);
            return await _ownerRepository.GetByEmailAsync(email);
        }
        public async Task<Owner?> GetByPhoneAsync(string phone)
        {
            _logger.LogInformation("Retrieving owner with phone {Phone}", phone);
            return await _ownerRepository.GetByPhoneAsync(phone);
        }
        public async Task<Owner?> GetByIdWithPetsAsync(int id)
        {
            _logger.LogInformation("Retrieving owner with ID {Id} and their pets", id);
            return await _ownerRepository.GetByIdWithPetsAsync(id);
        }

        public async Task<IEnumerable<Owner>> GetAllWithPetsAsync()
        {
            _logger.LogInformation("Retrieving all owners with their pets");
            return await _ownerRepository.GetAllWithPetsAsync();
        }
        public async Task<Owner> CreateAsync(Owner owner)
        {
            var existingEmail = await _ownerRepository.GetByEmailAsync(owner.Email);
            if (existingEmail != null)
            {
                _logger.LogWarning("Owner with email {Email} already exists", owner.Email);
                throw new InvalidOperationException($"Ya existe un propietario con el email {owner.Email}");
            }
            _logger.LogInformation("Creating new owner");
            return await _ownerRepository.CreateAsync(owner);
            
        }
        public async Task   UpdateAsync(int id, Owner owner)
        {
            var existingOwner = await GetByIdAsync(id);
            if (existingOwner == null)
            {
                _logger.LogWarning("Owner with ID {OwnerId} not found for update", id);
                throw new KeyNotFoundException(
                    $"No se encontró un propietario con el ID {id}");
            }

            var existing = existingOwner;
            existing.FirstName = owner.FirstName;
            existing.LastName = owner.LastName;
            existing.Phone = owner.Phone;
            existing.Email = owner.Email;
            _logger.LogInformation("Updating owner with ID {Id}", id);


            await _ownerRepository.UpdateAsync(existing);
            
        }
        public async Task DeleteAsync(int id)
        {
            var existingOwner = await GetByIdAsync(id);
            if (existingOwner == null)
            {
                _logger.LogWarning("Owner with ID {OwnerId} not found for deletion", id);
                throw new KeyNotFoundException(
                    $"No se encontró un propietario con el ID {id}");
            }
            _logger.LogInformation("Deleting owner with ID {Id}", id);
            await _ownerRepository.DeleteAsync(id);

        }
    }
}
