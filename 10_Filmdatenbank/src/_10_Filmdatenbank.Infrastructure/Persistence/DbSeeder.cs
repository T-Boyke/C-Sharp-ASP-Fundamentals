using _10_Filmdatenbank.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        // 1. Tags / Eigenschaften
        if (!await context.Eigenschaften.AnyAsync())
        {
            var tags = new List<Eigenschaft>
            {
                new Eigenschaft { Bezeichnung = "Actor" },
                new Eigenschaft { Bezeichnung = "Director" },
                new Eigenschaft { Bezeichnung = "Producer" }
            };
            context.Eigenschaften.AddRange(tags);
            await context.SaveChangesAsync();
        }

        // 2. Genres
        if (!await context.Genres.AnyAsync())
        {
            var genres = new List<Genre>
            {
                new Genre { Name = "Action" },
                new Genre { Name = "Sci-Fi" },
                new Genre { Name = "Drama" }
            };
            context.Genres.AddRange(genres);
            await context.SaveChangesAsync();
        }

        // 3. Personen
        if (!await context.Personen.AnyAsync())
        {
            var nolan = new Person { Vorname = "Christopher", Nachname = "Nolan" };
            var bale = new Person { Vorname = "Christian", Nachname = "Bale" };
            context.Personen.AddRange(nolan, bale);
            await context.SaveChangesAsync();
        }

        // 4. Filme
        if (!await context.Filme.AnyAsync())
        {
            var genres = await context.Genres.ToListAsync();
            var tags = await context.Eigenschaften.ToListAsync();
            var persons = await context.Personen.ToListAsync();

            var inception = new Film
            {
                Titel = "Inception",
                Handlung = "Ein Dieb, der Geheimnisse steuert.",
                Erscheinungsjahr = 2010,
                Preis = 9.99m,
                Genres = new List<Genre> { genres[0], genres[1] }
            };
            context.Filme.Add(inception);
            await context.SaveChangesAsync();

            // 5. Verbindungen (Reload to get IDs if needed, though Add should set them)
            var nolanRel = await context.Personen.FirstOrDefaultAsync(p => p.Nachname == "Nolan");
            var baleRel = await context.Personen.FirstOrDefaultAsync(p => p.Nachname == "Bale");
            var directorTagRel = await context.Eigenschaften.FirstOrDefaultAsync(t => t.Bezeichnung == "Director");
            var actorTagRel = await context.Eigenschaften.FirstOrDefaultAsync(t => t.Bezeichnung == "Actor");

            if (nolanRel != null && directorTagRel != null)
            {
                context.PersonEigenschaftFilme.Add(new PersonEigenschaftFilm
                {
                    FilmID = inception.FilmID,
                    PersonID = nolanRel.PersonID,
                    EigenschaftID = directorTagRel.EigenschaftID
                });
            }

            if (baleRel != null && actorTagRel != null)
            {
                context.PersonEigenschaftFilme.Add(new PersonEigenschaftFilm
                {
                    FilmID = inception.FilmID,
                    PersonID = baleRel.PersonID,
                    EigenschaftID = actorTagRel.EigenschaftID
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
