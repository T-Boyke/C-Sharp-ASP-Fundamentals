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
    public DbSet<Medication> Medications { get; set; } = null!;
    
    public DbSet<HealthInsurance> HealthInsurances { get; set; } = null!;
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Address> Addresses { get; set; } = null!;
    public DbSet<ContactInfo> ContactInfos { get; set; } = null!;

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

            entity.HasMany(p => p.Medications)
                  .WithOne(m => m.Patient)
                  .HasForeignKey(m => m.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.HealthInsurance)
                  .WithMany(h => h.Patients)
                  .HasForeignKey(p => p.HealthInsuranceId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Doctor)
                  .WithMany(d => d.Patients)
                  .HasForeignKey(p => p.DoctorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Address)
                  .WithMany(a => a.Patients)
                  .HasForeignKey(p => p.AddressId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(p => p.ContactInfo)
                  .WithMany(c => c.Patients)
                  .HasForeignKey(p => p.ContactInfoId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Seed-Daten werden über den DbSeeder bereitgestellt
    }
}
