using _08_Patienten2.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _08_Patienten2.Infrastructure.Persistence;

/// <summary>
/// Der Datenbankkontext der Anwendung, integriert mit ASP.NET Core Identity.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext(options)
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Examination> Examinations => Set<Examination>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<HealthInsurance> HealthInsurances => Set<HealthInsurance>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // DDD Mapping: Value Objects als Owned Types
        builder.Entity<Patient>(entity =>
        {
            entity.OwnsOne(p => p.Address);
            entity.OwnsOne(p => p.ContactInfo);
            
            // Konfiguration für IReadOnlyCollection
            entity.Metadata.FindNavigation(nameof(Patient.Examinations))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        });

        // Fluent API Mappings
        builder.Entity<Patient>().Property(p => p.Firstname).IsRequired().HasMaxLength(50);
        builder.Entity<Patient>().Property(p => p.Lastname).IsRequired().HasMaxLength(50);
        builder.Entity<Patient>().Property(p => p.SocialSecurityNumber).IsRequired().HasMaxLength(10);
    }
}
