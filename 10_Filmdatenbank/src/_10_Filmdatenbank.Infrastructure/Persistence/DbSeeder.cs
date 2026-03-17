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
        if (await context.Filme.AnyAsync()) return;

        // 1. Tags / Eigenschaften
        var tags = new List<Eigenschaft>
        {
            new Eigenschaft { Bezeichnung = "Actor" },
            new Eigenschaft { Bezeichnung = "Director" },
            new Eigenschaft { Bezeichnung = "Producer" }
        };
        context.Eigenschaften.AddRange(tags);

        // 2. Genres
        var genres = new List<Genre>
        {
            new Genre { Name = "Action" },
            new Genre { Name = "Sci-Fi" },
            new Genre { Name = "Drama" }
        };
        context.Genres.AddRange(genres);

        // 3. Personen
        var nolan = new Person { Vorname = "Christopher", Nachname = "Nolan" };
        var bale = new Person { Vorname = "Christian", Nachname = "Bale" };
        context.Personen.AddRange(nolan, bale);
        await context.SaveChangesAsync();

        // 4. Filme
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

        // 5. Verbindungen
        context.PersonEigenschaftFilme.Add(new PersonEigenschaftFilm
        {
            FilmID = inception.FilmID,
            PersonID = nolan.PersonID,
            EigenschaftID = tags[1].EigenschaftID
        });
        context.PersonEigenschaftFilme.Add(new PersonEigenschaftFilm
        {
            FilmID = inception.FilmID,
            PersonID = bale.PersonID,
            EigenschaftID = tags[0].EigenschaftID
        });

        await context.SaveChangesAsync();
    }
}
