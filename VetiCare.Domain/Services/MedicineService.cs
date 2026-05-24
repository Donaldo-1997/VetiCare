using Microsoft.Extensions.Logging;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;
using VetiCare.Domain.Interfaces.Services;

namespace VetiCare.Domain.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly ILogger<MedicineService> _logger;

        public MedicineService(IMedicineRepository medicineRepository, ILogger<MedicineService> logger)
        {
            _medicineRepository = medicineRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Medicine>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all medicines");
            return await _medicineRepository.GetAllAsync();
        }

        public async Task<Medicine?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving medicine with ID {Id}", id);
            return await _medicineRepository.GetByIdAsync(id);
        }

        public async Task<Medicine> GetByNameAsync(string name)
        {
            _logger.LogInformation("Retrieving medicine with name {Name}", name);
            return await _medicineRepository.GetByNameAsync(name);
        }

        public async Task<Medicine> CreateAsync(Medicine medicine)
        {
            var existingName = await _medicineRepository.GetByNameAsync(medicine.Name);
            if (existingName != null)
            {
                _logger.LogWarning("Medicine with name {Name} already exists", medicine.Name);
                throw new InvalidOperationException($"Ya existe un medicamento con el nombre {medicine.Name}");
            }

            _logger.LogInformation("Creating new medicine");
            return await _medicineRepository.CreateAsync(medicine);
        }

        public async Task UpdateAsync(int id, Medicine medicine)
        {
            var existingMedicine = await GetByIdAsync(id);
            if (existingMedicine == null)
            {
                _logger.LogWarning("Medicine with ID {MedicineId} not found for update", id);
                throw new KeyNotFoundException(
                    $"No se encontró un medicamento con el ID {id}");
            }

            var existingName = await _medicineRepository.GetByNameAsync(medicine.Name);
            if (existingName != null && existingName.Id != id)
            {
                throw new InvalidOperationException($"Ya existe un medicamento con el nombre {medicine.Name}");
            }

            var existing = existingMedicine;
            existing.Name = medicine.Name;
            existing.ActiveIngredient = medicine.ActiveIngredient;
            existing.Unit = medicine.Unit;

            _logger.LogInformation("Updating medicine with ID {MedicineId}", id);
            await _medicineRepository.UpdateAsync(existing);
        }
        public async Task DeleteAsync(int id)
        {
            var medicineExists = await _medicineRepository.ExistsAsync(id);
            if (!medicineExists)
            {
                _logger.LogWarning("Medicine with ID {MedicineId} not found for deletion", id);
                throw new KeyNotFoundException(
                    $"No se encontró un medicamento con el ID {id}");
            }
            _logger.LogInformation("Deleting medicine, ID: {MedicineId}", id);
            await _medicineRepository.DeleteAsync(id);
        }
    }
}
