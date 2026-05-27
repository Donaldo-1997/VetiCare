using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VetiCare.Domain.Entities;

namespace VetiCare.DataAccess.Context
{
    public class VetiCareDbContext :  IdentityDbContext<AppUser>
    {
        public VetiCareDbContext(DbContextOptions<VetiCareDbContext> options)
            : base(options)
        {
        }

        // --- DbSets (una propiedad por entidad) ---
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Forzar nombres de tabla en plural y correctos
            modelBuilder.Entity<Owner>().ToTable("Owners");
            modelBuilder.Entity<Pet>().ToTable("Pets");
            modelBuilder.Entity<Breed>().ToTable("Breeds");
            modelBuilder.Entity<Vet>().ToTable("Vets");
            modelBuilder.Entity<Appointment>().ToTable("Appointments");
            modelBuilder.Entity<MedicalRecord>().ToTable("MedicalRecords");
            modelBuilder.Entity<Medicine>().ToTable("Medicines");
            modelBuilder.Entity<Prescription>().ToTable("Prescriptions");
        }
    }
}
