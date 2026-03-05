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
            new Patient { Firstname = "Max", Lastname = "Mustermann", Birthdate = new DateTime(1985, 10, 20), SocialSecurityNumber = "9876201085" },
            new Patient { Firstname = "Erika", Lastname = "Musterfrau", Birthdate = new DateTime(1995, 3, 12), SocialSecurityNumber = "4567120395" },
            new Patient { Firstname = "Lukas", Lastname = "Weber", Birthdate = new DateTime(1978, 4, 5), SocialSecurityNumber = "1029050478" },
            new Patient { Firstname = "Sarah", Lastname = "Schmidt", Birthdate = new DateTime(1992, 8, 17), SocialSecurityNumber = "3847170892" },
            new Patient { Firstname = "Jonas", Lastname = "Müller", Birthdate = new DateTime(2005, 12, 1), SocialSecurityNumber = "5928011205" },
            new Patient { Firstname = "Anna", Lastname = "Fischer", Birthdate = new DateTime(1980, 1, 30), SocialSecurityNumber = "2837300180" },
            new Patient { Firstname = "Tim", Lastname = "Meyer", Birthdate = new DateTime(1998, 6, 22), SocialSecurityNumber = "4958220698" },
            new Patient { Firstname = "Lea", Lastname = "Wagner", Birthdate = new DateTime(1965, 11, 11), SocialSecurityNumber = "5829111165" },
            new Patient { Firstname = "Felix", Lastname = "Schulz", Birthdate = new DateTime(1988, 3, 25), SocialSecurityNumber = "6938250388" },
            new Patient { Firstname = "Marie", Lastname = "Becker", Birthdate = new DateTime(2010, 7, 14), SocialSecurityNumber = "7049140710" },
            new Patient { Firstname = "Julian", Lastname = "Hoffmann", Birthdate = new DateTime(1972, 9, 3), SocialSecurityNumber = "8150030972" },
            new Patient { Firstname = "Sophie", Lastname = "Schäfer", Birthdate = new DateTime(1994, 2, 28), SocialSecurityNumber = "9261280294" },
            new Patient { Firstname = "Mark", Lastname = "Koch", Birthdate = new DateTime(1950, 12, 24), SocialSecurityNumber = "1372241250" },
            new Patient { Firstname = "Elena", Lastname = "Bauer", Birthdate = new DateTime(2001, 5, 19), SocialSecurityNumber = "2483190501" },
            new Patient { Firstname = "David", Lastname = "Richter", Birthdate = new DateTime(1960, 10, 10), SocialSecurityNumber = "3594101060" },
            new Patient { Firstname = "Laura", Lastname = "Klein", Birthdate = new DateTime(1987, 1, 1), SocialSecurityNumber = "4605010187" },
            new Patient { Firstname = "Simon", Lastname = "Wolf", Birthdate = new DateTime(1999, 4, 15), SocialSecurityNumber = "5716150499" },
            new Patient { Firstname = "Julia", Lastname = "Schröder", Birthdate = new DateTime(1975, 8, 8), SocialSecurityNumber = "6827080875" },
            new Patient { Firstname = "Niklas", Lastname = "Neumann", Birthdate = new DateTime(1991, 11, 20), SocialSecurityNumber = "7938201191" },
            new Patient { Firstname = "Hannah", Lastname = "Schwarz", Birthdate = new DateTime(1983, 3, 14), SocialSecurityNumber = "8049140383" },
            new Patient { Firstname = "Paul", Lastname = "Zimmermann", Birthdate = new DateTime(1968, 6, 30), SocialSecurityNumber = "9150300668" },
            new Patient { Firstname = "Clara", Lastname = "Braun", Birthdate = new DateTime(2003, 9, 21), SocialSecurityNumber = "1261210903" }
        };

        await context.Patients.AddRangeAsync(patients);
        await context.SaveChangesAsync();
    }
}
