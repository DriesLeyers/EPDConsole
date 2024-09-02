using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole
{
    public class EPDDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=epd.db");
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(e =>
            {
                e.HasIndex(p => p.NationalRegisterNumber).IsUnique();
            });

            modelBuilder.Entity<Appointment>(e =>
            {
                e.HasOne(p => p.Patient).WithMany(p => p.Appointments);
                e.HasOne(p => p.Physician).WithMany(p => p.Appointments);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
