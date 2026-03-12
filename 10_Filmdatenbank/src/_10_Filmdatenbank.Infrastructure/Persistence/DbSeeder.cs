using System;
using System.Linq;
using System.Threading.Tasks;
using _10_Filmdatenbank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Infrastructure.Persistence;

/// <summary>
/// Hilfsklasse zum Befüllen der Datenbank mit initialen Testdaten.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Befüllt die Datenbank asynchron mit Testdaten, falls diese noch keine Filme enthält.
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Filme.AnyAsync()) return;

        // --- Eigenschaften (Rollen) ---
        // Diese werden bereits via OnModelCreating (Seed) angelegt:
        // 1: Regisseur, 2: Produzent, 3: Schauspieler

        // --- Personen (Real Data) ---
        var persons = new List<Person>
        {
            new Person { Vorname = "Keanu", Nachname = "Reeves", Biografie = "Kanadischer Schauspieler, bekannt für 'Matrix' und 'John Wick'.", Geburtsdatum = new DateTime(1964, 9, 2), ProfilBildUrl = "https://image.tmdb.org/t/p/w500/bb9797A9Arxt9I7S79UvM9vIayp.jpg" },
            new Person { Vorname = "Lana", Nachname = "Wachowski", Biografie = "US-amerikanische Regisseurin, Drehbuchautorin und Produzentin.", Geburtsdatum = new DateTime(1965, 6, 21), ProfilBildUrl = "https://image.tmdb.org/t/p/w500/775pQ4tV1i8pLd9H9wW5yVl0J5s.jpg" },
            new Person { Vorname = "Christopher", Nachname = "Nolan", Biografie = "Britisch-US-amerikanischer Regisseur von Blockbustern wie 'Inception'.", Geburtsdatum = new DateTime(1970, 7, 30), ProfilBildUrl = "https://image.tmdb.org/t/p/w500/9692vP5LidT0d74lXW4V9dF5hG5.jpg" },
            new Person { Vorname = "Leonardo", Nachname = "DiCaprio", Biografie = "Oskar-prämierter US-Schauspieler ('Titanic', 'Inception').", Geburtsdatum = new DateTime(1974, 11, 11), ProfilBildUrl = "https://image.tmdb.org/t/p/w500/wo2h4p04oo6quuup6o9v6u8ov0s.jpg" },
            new Person { Vorname = "Christian", Nachname = "Bale", Biografie = "Wandlungsfähiger britischer Schauspieler, verkörperte Batman.", Geburtsdatum = new DateTime(1974, 1, 30), ProfilBildUrl = "https://image.tmdb.org/t/p/w500/oEks7D9e5T6v6t6o9v6u8ov0s.jpg" },
            new Person { Vorname = "Matthew", Nachname = "McConaughey", Biografie = "US-Schauspieler, bekannt für dramatische Rollen ('Interstellar').", Geburtsdatum = new DateTime(1969, 11, 4), ProfilBildUrl = "https://image.tmdb.org/t/p/w500/6oKks7D9e5T6v6t6o9v6u8ov0s.jpg" },
            new Person { Vorname = "Quentin", Nachname = "Tarantino", Biografie = "Kultregisseur, bekannt für nicht-lineare Erzählweisen.", Geburtsdatum = new DateTime(1963, 3, 27), ProfilBildUrl = "https://image.tmdb.org/t/p/w500/3oKks7D9e5T6v6t6o9v6u8ov0s.jpg" },
            new Person { Vorname = "John", Nachname = "Travolta", Biografie = "US-Schauspieler, Feierte Comeback mit 'Pulp Fiction'.", Geburtsdatum = new DateTime(1954, 2, 18), ProfilBildUrl = "https://image.tmdb.org/t/p/w500/4oKks7D9e5T6v6t6o9v6u8ov0s.jpg" }
        };

        context.Personen.AddRange(persons);
        await context.SaveChangesAsync();

        // --- Filme (Real Data) ---
        var films = new List<Film>
        {
            new Film 
            { 
                Titel = "Matrix", 
                Erscheinungsjahr = 1999, 
                Erscheinungsdatum = new DateTime(1999, 6, 17),
                Spieldauer = 136, 
                Preis = 9.99m, 
                Genre = "Action, Sci-Fi",
                FskRating = "FSK 16",
                Nutzerwertung = 8.7,
                Handlung = "Ein Computerhacker erfährt von mysteriösen Rebellen über die wahre Natur seiner Realität und seine Rolle im Krieg gegen deren Kontrolleure.",
                PosterUrl = "https://image.tmdb.org/t/p/w500/f89U3Y9pS7yV6F8vH9w5Vl0J5s.jpg"
            },
            new Film 
            { 
                Titel = "Inception", 
                Erscheinungsjahr = 2010, 
                Erscheinungsdatum = new DateTime(2010, 7, 29),
                Spieldauer = 148, 
                Preis = 12.99m, 
                Genre = "Sci-Fi, Thriller",
                FskRating = "FSK 12",
                Nutzerwertung = 8.8,
                Handlung = "Ein Dieb, der Firmengeheimnisse durch den Einsatz von Traum-Sharing-Technologie stiehlt, erhält die umgekehrte Aufgabe, eine Idee in den Geist eines C.E.O. zu pflanzen.",
                PosterUrl = "https://image.tmdb.org/t/p/w500/9gk7p9vS7yV6F8vH9w5Vl0J5s.jpg"
            },
            new Film 
            { 
                Titel = "Interstellar", 
                Erscheinungsjahr = 2014, 
                Erscheinungsdatum = new DateTime(2014, 11, 6),
                Spieldauer = 169, 
                Preis = 14.99m, 
                Genre = "Abenteuer, Drama, Sci-Fi",
                FskRating = "FSK 12",
                Nutzerwertung = 8.7,
                Handlung = "Ein Team von Entdeckern reist durch ein Wurmloch im Weltraum, um das Überleben der Menschheit zu sichern.",
                PosterUrl = "https://image.tmdb.org/t/p/w500/gEU2Q6Y9pS7yV6F8vH9w5Vl0J5s.jpg"
            },
            new Film 
            { 
                Titel = "The Dark Knight", 
                Erscheinungsjahr = 2008, 
                Erscheinungsdatum = new DateTime(2008, 8, 21),
                Spieldauer = 152, 
                Preis = 9.99m, 
                Genre = "Action, Crime, Drama",
                FskRating = "FSK 16",
                Nutzerwertung = 9.0,
                Handlung = "Als die Bedrohung, die als der Joker bekannt ist, Chaos über Gotham City bringt, muss Batman eine seiner größten psychologischen und physischen Prüfungen bestehen.",
                PosterUrl = "https://image.tmdb.org/t/p/w500/qJ2t6Y9pS7yV6F8vH9w5Vl0J5s.jpg"
            },
            new Film 
            { 
                Titel = "Pulp Fiction", 
                Erscheinungsjahr = 1994, 
                Erscheinungsdatum = new DateTime(1994, 11, 3),
                Spieldauer = 154, 
                Preis = 7.99m, 
                Genre = "Crime, Drama",
                FskRating = "FSK 16",
                Nutzerwertung = 8.9,
                Handlung = "Das Leben zweier Auftragskiller, eines Boxers, der Frau eines Gangsters und zweier Diner-Räuber verflicht sich in vier Geschichten von Gewalt und Erlösung.",
                PosterUrl = "https://image.tmdb.org/t/p/w500/f79U3Y9pS7yV6F8vH9w5Vl0J5s.jpg"
            }
        };

        context.Filme.AddRange(films);
        await context.SaveChangesAsync();

        // --- Verknüpfungen (Join-Table) ---
        var dbMatrix = await context.Filme.FirstAsync(f => f.Titel == "Matrix");
        var dbInception = await context.Filme.FirstAsync(f => f.Titel == "Inception");
        var dbInterstellar = await context.Filme.FirstAsync(f => f.Titel == "Interstellar");
        var dbDarkKnight = await context.Filme.FirstAsync(f => f.Titel == "The Dark Knight");
        var dbPulpFiction = await context.Filme.FirstAsync(f => f.Titel == "Pulp Fiction");

        var dbKeanu = await context.Personen.FirstAsync(p => p.Vorname == "Keanu");
        var dbLana = await context.Personen.FirstAsync(p => p.Vorname == "Lana");
        var dbNolan = await context.Personen.FirstAsync(p => p.Vorname == "Christopher");
        var dbLeo = await context.Personen.FirstAsync(p => p.Vorname == "Leonardo");
        var dbBale = await context.Personen.FirstAsync(p => p.Vorname == "Christian");
        var dbMatthew = await context.Personen.FirstAsync(p => p.Vorname == "Matthew");
        var dbTarantino = await context.Personen.FirstAsync(p => p.Vorname == "Quentin");
        var dbTravolta = await context.Personen.FirstAsync(p => p.Vorname == "John");

        var links = new List<PersonEigenschaftFilm>
        {
            // Matrix
            new PersonEigenschaftFilm { FilmID = dbMatrix.FilmID, PersonID = dbKeanu.PersonID, EigenschaftID = 3 },
            new PersonEigenschaftFilm { FilmID = dbMatrix.FilmID, PersonID = dbLana.PersonID, EigenschaftID = 1 },
            // Inception
            new PersonEigenschaftFilm { FilmID = dbInception.FilmID, PersonID = dbLeo.PersonID, EigenschaftID = 3 },
            new PersonEigenschaftFilm { FilmID = dbInception.FilmID, PersonID = dbNolan.PersonID, EigenschaftID = 1 },
            // Interstellar
            new PersonEigenschaftFilm { FilmID = dbInterstellar.FilmID, PersonID = dbMatthew.PersonID, EigenschaftID = 3 },
            new PersonEigenschaftFilm { FilmID = dbInterstellar.FilmID, PersonID = dbNolan.PersonID, EigenschaftID = 1 },
            // The Dark Knight
            new PersonEigenschaftFilm { FilmID = dbDarkKnight.FilmID, PersonID = dbBale.PersonID, EigenschaftID = 3 },
            new PersonEigenschaftFilm { FilmID = dbDarkKnight.FilmID, PersonID = dbNolan.PersonID, EigenschaftID = 1 },
            // Pulp Fiction
            new PersonEigenschaftFilm { FilmID = dbPulpFiction.FilmID, PersonID = dbTravolta.PersonID, EigenschaftID = 3 },
            new PersonEigenschaftFilm { FilmID = dbPulpFiction.FilmID, PersonID = dbTarantino.PersonID, EigenschaftID = 1 }
        };

        context.PersonEigenschaftFilme.AddRange(links);
        await context.SaveChangesAsync();
    }
}
