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

        var patients = new List<Patient>();
        var random = new Random(42);

        string[] firstnames = { "Tobias", "Max", "Erika", "Lukas", "Sarah", "Jonas", "Anna", "Tim", "Lea", "Felix", "Marie", "Julian", "Sophie", "Mark", "Elena", "David", "Laura", "Simon", "Julia", "Niklas", "Hannah", "Paul", "Clara", "Andreas", "Barbara", "Christian", "Daniela", "Erik", "Frauke", "Gerhard", "Helga", "Ingo", "Johanna", "Karin", "Lothar", "Monika", "Norbert", "Olga", "Peter", "Renate", "Stefan", "Tanja", "Ulrich", "Ursula", "Volker", "Winfried", "Xaver", "Yvonne", "Zita" };
        string[] lastnames = { "Boyke", "Mustermann", "Musterfrau", "Weber", "Schmidt", "Müller", "Fischer", "Meyer", "Wagner", "Schulz", "Becker", "Hoffmann", "Schäfer", "Koch", "Bauer", "Richter", "Klein", "Wolf", "Schröder", "Neumann", "Schwarz", "Zimmermann", "Braun", "Krüger", "Hofmann", "Hartmann", "Lange", "Schmitt", "Werner", "Schmitz", "Krause", "Meier", "Lehmann", "Schmid", "Schulze", "Maier", "Köhler", "Herrmann", "König", "Walter", "Mayer", "Huber", "Kaiser", "Fuchs", "Peters", "Lang", "Scholz", "Möller", "Weiß" };
        string[] symptomsList = { "Kopfschmerzen, Übelkeit", "Rückenschmerzen (LWS)", "Husten, Schnupfen", "Erschöpfung, Schwindel", "Knieschmerzen links", "Bluthochdruck", "Schlafstörungen", "Magen-Darm-Beschwerden", "Allergische Reaktion", "Hautausschlag" };
        string[] medNames = { "Ibuprofen 400mg", "Paracetamol 500mg", "Amoxicillin 1000mg", "Pantoprazol 20mg", "L-Thyroxin 75", "Metformin 500mg", "Ramipril 5mg", "Simvastatin 20mg", "Bisoprolol 5mg", "D3-Vigantolol" };

        for (int i = 0; i < 50; i++)
        {
            var fn = i < firstnames.Length ? firstnames[i] : $"Patient_F_{i}";
            var ln = i < lastnames.Length ? lastnames[i] : $"Patient_L_{i}";
            var birth = new DateTime(1950 + random.Next(60), random.Next(1, 13), random.Next(1, 28));
            var svnr = $"{random.Next(1000, 9999)}{birth:ddMMyy}";

            var patient = new Patient
            {
                Firstname = fn,
                Lastname = ln,
                Birthdate = birth,
                SocialSecurityNumber = svnr,
                IsPrivatePatient = random.Next(10) < 3, // 30% Privat
                Symptoms = random.Next(10) < 7 ? symptomsList[random.Next(symptomsList.Length)] : null,
                NextAppointmentDate = random.Next(10) < 5 ? DateTime.Now.AddDays(random.Next(1, 30)).AddHours(random.Next(8, 17)) : null
            };

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

            patients.Add(patient);
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
