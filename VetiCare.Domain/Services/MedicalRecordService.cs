using Microsoft.Extensions.Logging;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;
using VetiCare.Domain.Interfaces.Services;

namespace VetiCare.Domain.Services
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly ILogger<MedicalRecordService> _logger;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, ILogger<MedicalRecordService> logger)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all medical records");
            return await _medicalRecordRepository.GetAllAsync();
        }

        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving medical record with ID: {MedicalRecordId}", id);
            var medicalRecord = await _medicalRecordRepository.GetByIdAsync(id);

            if (medicalRecord == null)
                _logger.LogWarning("Medical record with ID {MedicalRecordId} not found", id);

            return medicalRecord;

        }

        public async Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord)
        {
            _logger.LogInformation("Creating new medical record");
            return await _medicalRecordRepository.CreateAsync(medicalRecord);
        }

        public async Task UpdateAsync(int id, MedicalRecord medicalRecord)
        {
            var existingMedicalRecord = await _medicalRecordRepository.GetByIdAsync(id);
            if (existingMedicalRecord == null)
            {
                _logger.LogWarning("Medical record with ID {MedicalRecordId} not found for update", id);
                throw new InvalidOperationException($"No se encontró un registro médico con el ID {id}");
            }

            var existing = existingMedicalRecord;
            existing.Diagnosis = medicalRecord.Diagnosis;
            existing.Treatment = medicalRecord.Treatment;
            existing.Notes = medicalRecord.Notes;

            _logger.LogInformation("Updating medical record, ID: {MedicalRecordId}", medicalRecord.Id);
            await _medicalRecordRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var medicalRecordExists = await _medicalRecordRepository.ExistsAsync(id);
            if (!medicalRecordExists)
            {
                _logger.LogWarning("Medical record with ID {MedicalRecordId} not found for deletion", id);
                throw new InvalidOperationException($"No se encontró un registro médico con el ID {id}");
            }

            _logger.LogInformation("Deleting medical record, ID: {MedicalRecordId}", id);
            await _medicalRecordRepository.DeleteAsync(id);
        }

    }
}
