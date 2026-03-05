using Microsoft.EntityFrameworkCore;
using _07_Patienten.Domain.Entities;

namespace _07_Patienten.Infrastructure.Data;

/// <summary>
/// Datenbankkontext für die Patientenverwaltung.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Examination> Examinations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguration für Patient
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasMany(p => p.Examinations)
                  .WithOne(e => e.Patient)
                  .HasForeignKey(e => e.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed-Daten werden über den DbSeeder bereitgestellt
    }
}
