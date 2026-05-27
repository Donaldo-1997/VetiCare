using Microsoft.Extensions.Logging;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Enums;
using VetiCare.Domain.Interfaces.Repositories;
using VetiCare.Domain.Interfaces.Services;

namespace VetiCare.Domain.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todas las citas");
            return await _appointmentRepository.GetAllAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo cita con ID: {Id}", id);
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
                _logger.LogWarning("Cita con ID: {Id} no encontrada", id);

            return appointment;
        }

        public async Task<Appointment?> GetWithDetailsAsync(int id)
        {
            _logger.LogInformation("Obteniendo cita con detalles, ID: {Id}", id);
            var appointment = await _appointmentRepository.GetWithDetailsAsync(id);

            if (appointment == null)
                _logger.LogWarning("Cita con ID: {Id} no encontrada", id);

            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId)
        {
            _logger.LogInformation("Obteniendo citas de la mascota ID: {PetId}", petId);
            return await _appointmentRepository.GetByPetIdAsync(petId);
        }

        public async Task<IEnumerable<Appointment>> GetByVetIdAsync(int vetId)
        {
            _logger.LogInformation("Obteniendo citas del veterinario ID: {VetId}", vetId);
            return await _appointmentRepository.GetByVetIdAsync(vetId);
        }

        public async Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status)
        {
            _logger.LogInformation("Obteniendo citas con estado: {Status}", status);
            return await _appointmentRepository.GetByStatusAsync(status);
        }

        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            // Regla de negocio: no se puede agendar una cita en el pasado
            if (appointment.ScheduledAt <= DateTime.UtcNow)
                throw new InvalidOperationException("No se puede agendar una cita en una fecha y hora pasada.");

            _logger.LogInformation("Creando nueva cita para la mascota ID: {PetId}", appointment.PetId);
            return await _appointmentRepository.CreateAsync(appointment);
        }

        public async Task UpdateAsync(int id, Appointment appointment)
        {
            var existing = await _appointmentRepository.GetByIdAsync(id);

            if (existing == null)
            {
                _logger.LogWarning("Cita con ID: {Id} no encontrada para actualización", id);
                throw new KeyNotFoundException($"Cita con ID {id} no encontrada.");
            }

            // Regla de negocio: no se puede editar una cita cancelada o completada
            if (existing.Status == AppointmentStatus.Cancelled || existing.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException($"No se puede modificar una cita en estado '{existing.Status}'.");

            existing.ScheduledAt = appointment.ScheduledAt;
            existing.Reason = appointment.Reason;
            existing.PetId = appointment.PetId;
            existing.VetId = appointment.VetId;

            _logger.LogInformation("Actualizando cita con ID: {Id}", id);
            await _appointmentRepository.UpdateAsync(existing);
        }

        public async Task UpdateStatusAsync(int id, AppointmentStatus newStatus)
        {
            var existing = await _appointmentRepository.GetByIdAsync(id);

            if (existing == null)
            {
                _logger.LogWarning("Cita con ID: {Id} no encontrada para cambio de estado", id);
                throw new KeyNotFoundException($"Cita con ID {id} no encontrada.");
            }

            // Regla de negocio: transiciones de estado válidas
            var validTransitions = new Dictionary<AppointmentStatus, IEnumerable<AppointmentStatus>>
            {
                { AppointmentStatus.Scheduled,  new[] { AppointmentStatus.InProgress, AppointmentStatus.Cancelled } },
                { AppointmentStatus.InProgress, new[] { AppointmentStatus.Completed,  AppointmentStatus.Cancelled } },
                { AppointmentStatus.Completed,  Array.Empty<AppointmentStatus>() },
                { AppointmentStatus.Cancelled,  Array.Empty<AppointmentStatus>() }
            };

            if (!validTransitions[existing.Status].Contains(newStatus))
                throw new InvalidOperationException(
                    $"Transición de estado inválida: '{existing.Status}' → '{newStatus}'.");

            existing.Status = newStatus;
            _logger.LogInformation("Cita ID: {Id} — estado actualizado a '{Status}'", id, newStatus);
            await _appointmentRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _appointmentRepository.GetByIdAsync(id);

            if (existing == null)
            {
                _logger.LogWarning("Cita con ID: {Id} no encontrada para eliminación", id);
                throw new KeyNotFoundException($"Cita con ID {id} no encontrada.");
            }

            // Regla de negocio: no eliminar citas con historial médico
            var withDetails = await _appointmentRepository.GetWithDetailsAsync(id);
            if (withDetails?.MedicalRecords.Any() == true)
                throw new InvalidOperationException(
                    "No se puede eliminar una cita que tiene registros médicos asociados.");

            _logger.LogInformation("Eliminando cita con ID: {Id}", id);
            await _appointmentRepository.DeleteAsync(id);
        }
    }
}
