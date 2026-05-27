using Microsoft.Extensions.Logging;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;
using VetiCare.Domain.Interfaces.Services;

namespace VetiCare.Domain.Services
{
    public class VetService : IVetService
    {
        private readonly IVetRepository _vetRepository;
        private readonly ILogger<VetService> _logger;

        public VetService(IVetRepository vetRepository, ILogger<VetService> logger)
        {
            _vetRepository = vetRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Vet>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los veterinarios");
            return await _vetRepository.GetAllAsync();
        }

        public async Task<Vet?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo veterinario con ID: {Id}", id);
            var vet = await _vetRepository.GetByIdAsync(id);

            if (vet == null)
            {
                _logger.LogWarning("Veterinario con ID: {Id} no encontrado", id);
            }

            return vet;
        }

        public async Task<IEnumerable<Vet>> GetAllWithAppointmentsAsync()
        {
            _logger.LogInformation("Obteniendo todos los veterinarios con sus citas");
            return await _vetRepository.GetAllVetsWithAppointmentsAsync();
        }

        public async Task<Vet> CreateAsync(Vet vet)
        {
            _logger.LogInformation("Creando nuevo veterinario");
            return await _vetRepository.CreateAsync(vet);
        }

        public async Task UpdateAsync(int id, Vet vet)
        {
            var existingVet = await _vetRepository.GetByIdAsync(id);

            if (existingVet == null)
            {
                _logger.LogWarning("Veterinario con ID: {Id} no encontrado para actualización", id);
                throw new KeyNotFoundException($"Veterinario con ID {id} no encontrado");
            }
            existingVet.FirstName = vet.FirstName;
            existingVet.LastName = vet.LastName;
            existingVet.Specialty = vet.Specialty;
            existingVet.LicenseNumber = vet.LicenseNumber;

            _logger.LogInformation("Actualizando veterinario con ID: {Id}", id);
            await _vetRepository.UpdateAsync(existingVet);
        }

        public async Task DeleteAsync(int id)
        {
            var existingVet = await _vetRepository.GetByIdAsync(id);

            if (existingVet == null)
            {
                _logger.LogWarning("Veterinario con ID: {Id} no encontrado para eliminación", id);
                throw new KeyNotFoundException($"Veterinario con ID {id} no encontrado");
            }
            _logger.LogInformation("Eliminando veterinario con ID: {Id}", id);
            await _vetRepository.DeleteAsync(id);
        }
    }
}
