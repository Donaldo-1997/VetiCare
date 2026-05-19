using Microsoft.EntityFrameworkCore;
using VetiCare.Domain.Entities;

namespace VetiCare.DataAccess.Context
{
    public class VetiCareDbContext : DbContext
    {
        public VetiCareDbContext(DbContextOptions<VetiCareDbContext> options)
            : base(options)
        {
        }

        public DbSet<Owner> Teams => Set<Owner>();
        public DbSet<Vet> Players => Set<Vet>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

