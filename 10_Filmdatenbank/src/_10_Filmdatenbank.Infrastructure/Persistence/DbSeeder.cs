using _10_Filmdatenbank.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace _10_Filmdatenbank.Infrastructure.Persistence;

/// <summary>
/// Hilfsklasse zum Befüllen der Datenbank mit initialen Testdaten.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Befüllt die Datenbank mit initialen Daten.
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    /// <returns>Ein Task-Objekt.</returns>
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var logPath = @"C:\Users\Tobia\Desktop\cSharpRepo\C-Sharp-ASP-Fundamentals\10_Filmdatenbank\e2e_debug.log";
        File.AppendAllText(logPath, $"[{DateTime.Now}] [DEBUG] Seeding database...{Environment.NewLine}");

        // 1. Tags / Eigenschaften
        var directorTag = await context.Eigenschaften.FirstOrDefaultAsync(t => t.Bezeichnung == "Director");
        if (directorTag == null)
        {
            directorTag = new Eigenschaft { Bezeichnung = "Director" };
            context.Eigenschaften.Add(directorTag);
        }
        var actorTag = await context.Eigenschaften.FirstOrDefaultAsync(t => t.Bezeichnung == "Actor");
        if (actorTag == null)
        {
            actorTag = new Eigenschaft { Bezeichnung = "Actor" };
            context.Eigenschaften.Add(actorTag);
        }
        await context.SaveChangesAsync();

        // 2. Genres
        var actionGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Action");
        if (actionGenre == null)
        {
            actionGenre = new Genre { Name = "Action" };
            context.Genres.Add(actionGenre);
        }
        var sciFiGenre = await context.Genres.FirstOrDefaultAsync(g => g.Name == "Sci-Fi");
        if (sciFiGenre == null)
        {
            sciFiGenre = new Genre { Name = "Sci-Fi" };
            context.Genres.Add(sciFiGenre);
        }
        await context.SaveChangesAsync();

        // 3. Personen
        var nolan = await context.Personen.FirstOrDefaultAsync(p => p.Nachname == "Nolan");
        if (nolan == null)
        {
            nolan = new Person { Vorname = "Christopher", Nachname = "Nolan" };
            context.Personen.Add(nolan);
        }
        var bale = await context.Personen.FirstOrDefaultAsync(p => p.Nachname == "Bale");
        if (bale == null)
        {
            bale = new Person { Vorname = "Christian", Nachname = "Bale" };
            context.Personen.Add(bale);
        }
        await context.SaveChangesAsync();

        // 4. Filme
        var inception = await context.Filme.Include(f => f.Genres).Include(f => f.PersonEigenschaftFilme).FirstOrDefaultAsync(f => f.Titel == "Inception");
        if (inception == null)
        {
            inception = new Film
            {
                Titel = "Inception",
                Handlung = "Ein Dieb, der Geheimnisse steuert.",
                Erscheinungsjahr = 2010,
                Preis = 9.99m,
                Genres = new List<Genre> { actionGenre, sciFiGenre }
            };
            context.Filme.Add(inception);
            await context.SaveChangesAsync();
        }

        // 5. Verbindungen sicherstellen
        if (!context.PersonEigenschaftFilme.Any(pef => pef.FilmId == inception.Id && pef.PersonId == nolan.Id))
        {
            File.AppendAllText(logPath, $"[{DateTime.Now}] [DEBUG] Linking '{inception.Titel}' to Person '{nolan.Vorname} {nolan.Nachname}' as '{directorTag.Bezeichnung}'{Environment.NewLine}");
            context.PersonEigenschaftFilme.Add(new PersonEigenschaftFilm
            {
                Film = inception,
                Person = nolan,
                Eigenschaft = directorTag
            });
        }

        if (!context.PersonEigenschaftFilme.Any(pef => pef.FilmId == inception.Id && pef.PersonId == bale.Id))
        {
            File.AppendAllText(logPath, $"[{DateTime.Now}] [DEBUG] Linking '{inception.Titel}' to Person '{bale.Vorname} {bale.Nachname}' as '{actorTag.Bezeichnung}'{Environment.NewLine}");
            context.PersonEigenschaftFilm entry = new PersonEigenschaftFilm
            {
                Film = inception,
                Person = bale,
                Eigenschaft = actorTag
            };
            context.PersonEigenschaftFilme.Add(entry);
        }

        await context.SaveChangesAsync();
        File.AppendAllText(logPath, $"[{DateTime.Now}] [DEBUG] Seeding completed.{Environment.NewLine}");
    }
}
