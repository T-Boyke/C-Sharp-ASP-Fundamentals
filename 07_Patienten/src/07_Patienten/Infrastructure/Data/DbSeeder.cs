using Microsoft.EntityFrameworkCore;
using _07_Patienten.Domain.Entities;

namespace _07_Patienten.Infrastructure.Data;

/// <summary>
/// Hilfsklasse zur Initialisierung der Datenbank mit Testdaten.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Patients.AnyAsync()) return;

        var patients = new List<Patient>
        {
            new Patient 
            { 
                Firstname = "Tobia", 
                Lastname = "Boyke", 
                Birthdate = new DateTime(1990, 5, 15), 
                SocialSecurityNumber = "1234150590",
                Examinations = new List<Examination>
                {
                    new Examination { Date = DateTime.Now.AddDays(-10), Findings = "Routineuntersuchung: Alles ok." },
                    new Examination { Date = DateTime.Now.AddDays(-2), Findings = "Leichte Erkältung: Ruhe verordnet." }
                }
            },
            new Patient 
            { 
                Firstname = "Max", 
                Lastname = "Mustermann", 
                Birthdate = new DateTime(1985, 10, 20), 
                SocialSecurityNumber = "9876201085" 
            },
            new Patient 
            { 
                Firstname = "Erika", 
                Lastname = "Musterfrau", 
                Birthdate = new DateTime(1995, 3, 12), 
                SocialSecurityNumber = "4567120395" 
            }
        };

        await context.Patients.AddRangeAsync(patients);
        await context.SaveChangesAsync();
    }
}
