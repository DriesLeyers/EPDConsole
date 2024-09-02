using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole
{
    public class EPDDbContext : DbContext
    {
        // The following configures EF to create a Sqlite database file in the
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=epd.db");
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Patient>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.NationalRegisterNumber)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }


}
