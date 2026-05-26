using Microsoft.Extensions.Logging;
using System.Numerics;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Interfaces.Repositories;
using VetiCare.Domain.Interfaces.Services;

namespace VetiCare.Domain.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IMedicineRepository _medicineRepository;
        private readonly ILogger<PrescriptionService> _logger;
        public PrescriptionService(IPrescriptionRepository prescriptionRepository, IMedicalRecordRepository medicalRecordRepository, IMedicineRepository medicineRepository, ILogger<PrescriptionService> logger)
        {
            _prescriptionRepository = prescriptionRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _medicineRepository = medicineRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Prescription>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all prescriptions");
            return await _prescriptionRepository.GetAllAsync();
        }

        public async Task<Prescription?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving prescription with ID {Id}", id);
            return await _prescriptionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Prescription>> GetByMedicalRecordAsync(int medicalRecordId)
        {
            //validar que el registro médico exista
            var MedicalRecordExists = await _medicalRecordRepository.ExistsAsync(medicalRecordId);
            if (!MedicalRecordExists)
            {
                _logger.LogWarning("Medical Record with ID {MedicalRecordId} not found", medicalRecordId);
                throw new KeyNotFoundException(
                    $"No se encontró el Registro Médico con el ID {medicalRecordId}");
            }
            _logger.LogInformation("Retrieving prescriptions for Medical Record ID {MedicalRecordId}", medicalRecordId);
            return await _prescriptionRepository.GetbyMedicalRecordIdAsync(medicalRecordId);
        }

        public async Task<IEnumerable<Prescription>> GetByMedicineAsync(int medicineId)
        {
            var MedicineExists = await _medicineRepository.ExistsAsync(medicineId);
            if (!MedicineExists)
            {
                _logger.LogWarning("Medicine with ID {MedicineId} not found", medicineId);
                throw new KeyNotFoundException(
                    $"No se encontró la Medicina con el ID {medicineId}");
            }
            _logger.LogInformation("Retrieving prescriptions for Medicine ID {MedicineId}", medicineId);
            return await _prescriptionRepository.GetbyMedicineIdAsync(medicineId);
        }

        public async Task<Prescription?> GetByIdWithDetailsAsync(int id)
        {
            return await _prescriptionRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<Prescription> CreateAsync(Prescription prescription)
        {
            // Validar que el historial médico exista
            var medicalRecordExists = await _medicalRecordRepository.ExistsAsync(prescription.MedicalRecordId);
            if (!medicalRecordExists)
            {
                _logger.LogWarning("Medical Record with ID {MedicalRecordId} not found", prescription.MedicalRecordId);
                throw new KeyNotFoundException(
                    $"No se encontró el Registro Médico con el ID {prescription.MedicalRecordId}");
            }

            // Validar que la medicina exista
            var medicineExists = await _medicineRepository.ExistsAsync(prescription.MedicineId);
            if (!medicineExists)
            {
                _logger.LogWarning("Medicine with ID {MedicineId} not found", prescription.MedicineId);
                throw new KeyNotFoundException(
                    $"No se encontró la Medicina con el ID {prescription.MedicineId}");
            }


            _logger.LogInformation("Creating a new prescription");
            return await _prescriptionRepository.CreateAsync(prescription);
        }

        public async Task UpdateAsync(int id, Prescription prescription)
        {
            var existingPrescription = await _prescriptionRepository.GetByIdAsync(id);
            if (existingPrescription == null)
            {
                _logger.LogWarning("Prescription with ID {Id} not found for update", id);
                throw new KeyNotFoundException(
                    $"No se encontró la Prescripción con el ID {id}");
            }

            //Validar que el nuevo historial médico exista
            var medicalRecordExists = await _medicalRecordRepository.ExistsAsync(prescription.MedicalRecordId);
            if (!medicalRecordExists)
            {
                throw new KeyNotFoundException(
                    $"No se encontró el Registro Médico con el ID {prescription.MedicalRecordId}");
            }

            // Validar que la nueva medicina exista
            var medicineExists = await _medicineRepository.ExistsAsync(prescription.MedicineId);
            if (!medicineExists)
            {
                throw new KeyNotFoundException(
                    $"No se encontró la Medicina con el ID {prescription.MedicineId}");
            }

            existingPrescription.Quantity = prescription.Quantity;
            existingPrescription.Dosage = prescription.Dosage;
            existingPrescription.Instructions = prescription.Instructions;
            existingPrescription.MedicalRecordId = prescription.MedicalRecordId;
            existingPrescription.MedicineId = prescription.MedicineId;

            _logger.LogInformation("Updating prescription with ID {Id}", id);
            await _prescriptionRepository.UpdateAsync(existingPrescription);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _prescriptionRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Prescription with ID {Id} not found for deletion", id);
                throw new KeyNotFoundException(
                    $"No se encontró la Prescripción con el ID {id}");
            }
            _logger.LogInformation("Deleting prescription with ID: {PrescriptionId}", id);
            await _prescriptionRepository.DeleteAsync(id);


        }

    }
}
