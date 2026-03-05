using Bogus;
using Microsoft.EntityFrameworkCore;
using _07_Patienten.Domain.Entities;

namespace _07_Patienten.Infrastructure.Data;

/// <summary>
/// Hilfsklasse zur Initialisierung der Datenbank mit umfangreichen Testdaten.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Patients.AnyAsync()) return;

        var patientFaker = new Faker<Patient>("de")
            .RuleFor(p => p.Firstname, f => f.Name.FirstName())
            .RuleFor(p => p.Lastname, f => f.Name.LastName())
            .RuleFor(p => p.Birthdate, f => f.Date.Past(60, DateTime.Now.AddYears(-20)))
            .RuleFor(p => p.IsPrivatePatient, f => f.Random.Bool(0.3f))
            .RuleFor(p => p.Symptoms, f => f.Random.Bool(0.7f) ? f.PickRandom("Kopfschmerzen, Übelkeit", "Rückenschmerzen (LWS)", "Husten, Schnupfen", "Erschöpfung, Schwindel", "Knieschmerzen links", "Bluthochdruck", "Schlafstörungen", "Magen-Darm-Beschwerden", "Allergische Reaktion", "Hautausschlag") : null)
            .RuleFor(p => p.NextAppointmentDate, f => f.Random.Bool(0.5f) ? f.Date.Soon(30).Date.AddHours(f.Random.Int(8, 17)) : null);

        var patients = patientFaker.Generate(50);
        
        // Enhance patients with SVNR, examinations and medications
        string[] medNames = { "Ibuprofen 400mg", "Paracetamol 500mg", "Amoxicillin 1000mg", "Pantoprazol 20mg", "L-Thyroxin 75", "Metformin 500mg", "Ramipril 5mg", "Simvastatin 20mg", "Bisoprolol 5mg", "D3-Vigantolol" };
        var random = new Random(42);

        foreach (var patient in patients)
        {
            patient.SocialSecurityNumber = $"{random.Next(1000, 9999)}{patient.Birthdate:ddMMyy}";

            // Untersuchungen
            int examCount = random.Next(1, 5);
            for (int j = 0; j < examCount; j++)
            {
                patient.Examinations.Add(new Examination
                {
                    Date = DateTime.Now.AddDays(-random.Next(1, 100)),
                    Findings = $"Befund für Untersuchung {j + 1}: Alles im Normbereich."
                });
            }

            // Medikamente
            int medCount = random.Next(0, 3);
            for (int k = 0; k < medCount; k++)
            {
                patient.Medications.Add(new Medication
                {
                    Name = medNames[random.Next(medNames.Length)],
                    Dosage = "1-0-1",
                    Instructions = "Nach dem Essen einnehmen.",
                    PrescribedDate = DateTime.Now.AddDays(-random.Next(1, 50))
                });
            }
        }

        // Spezielle Testdaten für Tobias Boyke (falls nicht in Liste)
        if (!patients.Any(p => p.Lastname == "Boyke"))
        {
            patients.Add(new Patient
            {
                Firstname = "Tobias",
                Lastname = "Boyke",
                Birthdate = new DateTime(1986, 1, 16),
                SocialSecurityNumber = "1234150590",
                IsPrivatePatient = true,
                Symptoms = "Übermäßiger Energy-Drink Konsum",
                NextAppointmentDate = DateTime.Now.AddDays(2).AddHours(14),
                Examinations = new List<Examination> { new Examination { Date = DateTime.Now.AddDays(-1), Findings = "Blutdruck leicht erhöht (Koffein?)" } }
            });
        }

        await context.Patients.AddRangeAsync(patients);
        await context.SaveChangesAsync();
    }
}
