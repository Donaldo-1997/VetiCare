using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;
using VetiCare.Domain.Entities;
using VetiCare.Domain.Enums;
using VetiCare.Domain.Interfaces.Repositories;

namespace VetiCare.DataAccess.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(VetiCareDbContext context) : base(context)
        {
        }

        public async Task<Appointment?> GetWithDetailsAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Pet)
                    .ThenInclude(p => p.Owner)
                .Include(a => a.Vet)
                .Include(a => a.MedicalRecords)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetByPetIdAsync(int petId)
        {
            return await _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Vet)
                .Where(a => a.PetId == petId)
                .OrderByDescending(a => a.ScheduledAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByVetIdAsync(int vetId)
        {
            return await _context.Appointments
                .Include(a => a.Pet)
                    .ThenInclude(p => p.Owner)
                .Include(a => a.Vet)
                .Where(a => a.VetId == vetId)
                .OrderByDescending(a => a.ScheduledAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status)
        {
            return await _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Vet)
                .Where(a => a.Status == status)
                .OrderBy(a => a.ScheduledAt)
                .ToListAsync();
        }
    }
}
