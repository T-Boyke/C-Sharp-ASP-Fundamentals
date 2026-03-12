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
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Filme.AnyAsync()) return;

        // --- Role Mapping ---
        const int REGIE = 1;
        const int PRODUZENT = 2;
        const int SCHAUSPIELER = 3;

        // --- Persons Expansion ---
        var persons = new List<Person>
        {
            // Directors & Producers & Actors
            new Person { Vorname = "Christopher", Nachname = "Nolan", Biografie = "Britisch-US-amerikanischer Regisseur.", Geburtsdatum = new DateTime(1970, 7, 30), Geburtsort = "London, UK", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/9692vP5LidT0d74lXW4V9dF5hG5.jpg", Tags = "Genius, Nonlinear, IMAX" },
            new Person { Vorname = "Quentin", Nachname = "Tarantino", Biografie = "Kultregisseur.", Geburtsdatum = new DateTime(1963, 3, 27), Geburtsort = "Knoxville, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/3oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Dialogue, Violence, Feet" },
            new Person { Vorname = "Steven", Nachname = "Spielberg", Biografie = "Legendärer Regisseur.", Geburtsdatum = new DateTime(1946, 12, 18), Geburtsort = "Cincinnati, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/p25pxoFhYpS28Y7S89v6u8ov0s.jpg", Tags = "Blockbuster, Amblin, Legend" },
            new Person { Vorname = "Martin", Nachname = "Scorsese", Biografie = "Meister des Gangsterfilms.", Geburtsdatum = new DateTime(1942, 11, 17), Geburtsort = "New York City, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/6oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Crime, Catholic, Legend" },
            new Person { Vorname = "James", Nachname = "Cameron", Biografie = "Visionärer Regisseur.", Geburtsdatum = new DateTime(1954, 8, 16), Geburtsort = "Kapuskasing, Kanada", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/gEU2Q6Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Sci-Fi, Tech, Ocean" },
            new Person { Vorname = "Ridley", Nachname = "Scott", Biografie = "Britischer Regisseur.", Geburtsdatum = new DateTime(1937, 11, 30), Geburtsort = "South Shields, UK", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/9gk7p9vS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Sci-Fi, Visuals, Historical" },
            new Person { Vorname = "David", Nachname = "Fincher", Biografie = "Meister des Thrillers.", Geburtsdatum = new DateTime(1962, 8, 28), Geburtsort = "Denver, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/f89U3Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Perfectionist, Dark, Digital" },
            new Person { Vorname = "Francis Ford", Nachname = "Coppola", Biografie = "Regisseur von 'Der Pate'.", Geburtsdatum = new DateTime(1939, 4, 7), Geburtsort = "Detroit, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/wo2h4p04oo6quuup6o9v6u8ov0s.jpg", Tags = "New Hollywood, Legend" },
            
            // Actors
            new Person { Vorname = "Leonardo", Nachname = "DiCaprio", Biografie = "Oskar-prämierter Schauspieler.", Geburtsdatum = new DateTime(1974, 11, 11), Geburtsort = "Los Angeles, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/wo2h4p04oo6quuup6o9v6u8ov0s.jpg", Tags = "Method, Climate, Legend" },
            new Person { Vorname = "Brad", Nachname = "Pitt", Biografie = "Globaler Superstar.", Geburtsdatum = new DateTime(1963, 12, 18), Geburtsort = "Shawnee, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/6oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Charismatic, Producer" },
            new Person { Vorname = "Tom", Nachname = "Hanks", Biografie = "Amerikas beliebtester Schauspieler.", Geburtsdatum = new DateTime(1956, 7, 9), Geburtsort = "Concord, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/4oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Nicest, Everyman" },
            new Person { Vorname = "Morgan", Nachname = "Freeman", Biografie = "Die Stimme Gottes.", Geburtsdatum = new DateTime(1937, 6, 1), Geburtsort = "Memphis, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/bb9797A9Arxt9I7S79UvM9vIayp.jpg", Tags = "Voice, Legend" },
            new Person { Vorname = "Christian", Nachname = "Bale", Biografie = "Verkörperte Batman.", Geburtsdatum = new DateTime(1974, 1, 30), Geburtsort = "Haverfordwest, UK", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/oEks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Transformation, Intensity" },
            new Person { Vorname = "Matthew", Nachname = "McConaughey", Biografie = "Oscar-Gewinner.", Geburtsdatum = new DateTime(1969, 11, 4), Geburtsort = "Uvalde, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/6oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Alright, McConaissance" },
            new Person { Vorname = "Keanu", Nachname = "Reeves", Biografie = "Action-Legende.", Geburtsdatum = new DateTime(1964, 9, 2), Geburtsort = "Beirut, Libanon", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/bb9797A9Arxt9I7S79UvM9vIayp.jpg", Tags = "Wholesome, Cyberpunk" },
            new Person { Vorname = "Robert", Nachname = "De Niro", Biografie = "Schauspiel-Legende.", Geburtsdatum = new DateTime(1943, 8, 17), Geburtsort = "New York City, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/p25pxoFhYpS28Y7S89v6u8ov0s.jpg", Tags = "Method, Gangster" },
            new Person { Vorname = "Al", Nachname = "Pacino", Biografie = "Einer der Größten.", Geburtsdatum = new DateTime(1940, 4, 25), Geburtsort = "New York City, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/9692vP5LidT0d74lXW4V9dF5hG5.jpg", Tags = "Intensity, Legend" }
        };

        context.Personen.AddRange(persons);
        await context.SaveChangesAsync();

        // --- Films Expansion (30+) ---
        var films = new List<Film>
        {
            new Film { Titel = "Matrix", Erscheinungsjahr = 1999, Spieldauer = 136, Preis = 9.99m, Genre = "Action, Sci-Fi", FskRating = "FSK 16", Nutzerwertung = 8.7, Handlung = "Ein Computerhacker erfährt von mysteriösen Rebellen über die wahre Natur seiner Realität.", PosterUrl = "https://image.tmdb.org/t/p/w500/f89U3Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Cult, Simulation, Slow-Motion" },
            new Film { Titel = "Inception", Erscheinungsjahr = 2010, Spieldauer = 148, Preis = 12.99m, Genre = "Sci-Fi, Thriller", FskRating = "FSK 12", Nutzerwertung = 8.8, Handlung = "Diebe stehlen Geheimnisse durch Träume.", PosterUrl = "https://image.tmdb.org/t/p/w500/9gk7p9vS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Dreams, Mind-bending, Hans Zimmer" },
            new Film { Titel = "Interstellar", Erscheinungsjahr = 2014, Spieldauer = 169, Preis = 14.99m, Genre = "Abenteuer, Sci-Fi", FskRating = "FSK 12", Nutzerwertung = 8.7, Handlung = "Reise durch ein Wurmloch zur Rettung der Menschheit.", PosterUrl = "https://image.tmdb.org/t/p/w500/gEU2Q6Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Space, Wormhole, Tears" },
            new Film { Titel = "The Dark Knight", Erscheinungsjahr = 2008, Spieldauer = 152, Preis = 9.99m, Genre = "Action, Crime", FskRating = "FSK 16", Nutzerwertung = 9.0, Handlung = "Batman kämpft gegen den Joker.", PosterUrl = "https://image.tmdb.org/t/p/w500/qJ2t6Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Joker, Masterpiece, Batman" },
            new Film { Titel = "Pulp Fiction", Erscheinungsjahr = 1994, Spieldauer = 154, Preis = 7.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.9, Handlung = "Verflochtene Geschichten in Los Angeles.", PosterUrl = "https://image.tmdb.org/t/p/w500/f79U3Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Dialogue, Iconic, Nonlinear" },
            new Film { Titel = "Fight Club", Erscheinungsjahr = 1999, Spieldauer = 139, Preis = 8.99m, Genre = "Drama, Thriller", FskRating = "FSK 18", Nutzerwertung = 8.8, Handlung = "Ein unter Schlaflosigkeit leidender Mann gründet einen Boxclub.", PosterUrl = "https://image.tmdb.org/t/p/w500/wo2h4p04oo6quuup6o9v6u8ov0s.jpg", Tags = "Chaos, Twist, Soap" },
            new Film { Titel = "Forrest Gump", Erscheinungsjahr = 1994, Spieldauer = 142, Preis = 9.99m, Genre = "Drama, Romance", FskRating = "FSK 12", Nutzerwertung = 8.8, Handlung = "Das außergewöhnliche Leben eines einfachen Mannes.", PosterUrl = "https://image.tmdb.org/t/p/w500/4oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "History, Running, Chocolate" },
            new Film { Titel = "The Shawshank Redemption", Erscheinungsjahr = 1994, Spieldauer = 142, Preis = 9.99m, Genre = "Drama, Crime", FskRating = "FSK 12", Nutzerwertung = 9.3, Handlung = "Hoffnung hinter Gefängnismauern.", PosterUrl = "https://image.tmdb.org/t/p/w500/bb9797A9Arxt9I7S79UvM9vIayp.jpg", Tags = "Hope, Freedom, King" },
            new Film { Titel = "The Godfather", Erscheinungsjahr = 1972, Spieldauer = 175, Preis = 14.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 9.2, Handlung = "Der Aufstieg einer Mafia-Familie.", PosterUrl = "https://image.tmdb.org/t/p/w500/p25pxoFhYpS28Y7S89v6u8ov0s.jpg", Tags = "Mafia, Family, Masterpiece" },
            new Film { Titel = "Schindler's List", Erscheinungsjahr = 1993, Spieldauer = 195, Preis = 12.99m, Genre = "War, Drama", FskRating = "FSK 12", Nutzerwertung = 9.0, Handlung = "Rettung während des Holocausts.", PosterUrl = "https://image.tmdb.org/t/p/w500/9692vP5LidT0d74lXW4V9dF5hG5.jpg", Tags = "Holocaust, Black and White, Moving" },
            new Film { Titel = "Gladiator", Erscheinungsjahr = 2000, Spieldauer = 155, Preis = 9.99m, Genre = "Action, Adventure", FskRating = "FSK 16", Nutzerwertung = 8.5, Handlung = "Ein verratener General wird Gladiator.", PosterUrl = "https://image.tmdb.org/t/p/w500/gEU2Q6Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Rome, Revange, Epic" },
            new Film { Titel = "Avatar", Erscheinungsjahr = 2009, Spieldauer = 162, Preis = 12.99m, Genre = "Adventure, Sci-Fi", FskRating = "FSK 12", Nutzerwertung = 7.9, Handlung = "Kampf auf Pandora.", PosterUrl = "https://image.tmdb.org/t/p/w500/p25pxoFhYpS28Y7S89v6u8ov0s.jpg", Tags = "3D, Visuals, Pandora" },
            new Film { Titel = "Titanic", Erscheinungsjahr = 1997, Spieldauer = 194, Preis = 9.99m, Genre = "Drama, Romance", FskRating = "FSK 12", Nutzerwertung = 7.9, Handlung = "Liebe auf dem sinkenden Schiff.", PosterUrl = "https://image.tmdb.org/t/p/w500/wo2h4p04oo6quuup6o9v6u8ov0s.jpg", Tags = "Iceberg, Celine Dion, Epic" },
            new Film { Titel = "Jurassic Park", Erscheinungsjahr = 1993, Spieldauer = 127, Preis = 8.99m, Genre = "Adventure, Sci-Fi", FskRating = "FSK 12", Nutzerwertung = 8.2, Handlung = "Dinosaurier kehren zurück.", PosterUrl = "https://image.tmdb.org/t/p/w500/4oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Dinosaurs, Theme Park, Spielberg" },
            new Film { Titel = "Se7en", Erscheinungsjahr = 1995, Spieldauer = 127, Preis = 8.99m, Genre = "Crime, Thriller", FskRating = "FSK 16", Nutzerwertung = 8.6, Handlung = "Suche nach einem Serienmörder.", PosterUrl = "https://image.tmdb.org/t/p/w500/f89U3Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Seven Sins, Dark, Fincher" },
            new Film { Titel = "The Prestige", Erscheinungsjahr = 2006, Spieldauer = 130, Preis = 10.99m, Genre = "Drama, Mystery", FskRating = "FSK 12", Nutzerwertung = 8.5, Handlung = "Rivalisierende Magier.", PosterUrl = "https://image.tmdb.org/t/p/w500/9gk7p9vS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Magic, Rivalry, Nolan" },
            new Film { Titel = "Django Unchained", Erscheinungsjahr = 2012, Spieldauer = 165, Preis = 11.99m, Genre = "Western, Drama", FskRating = "FSK 16", Nutzerwertung = 8.4, Handlung = "Ein Sklave wird Kopfgeldjäger.", PosterUrl = "https://image.tmdb.org/t/p/w500/3oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Western, Django, Tarantino" },
            new Film { Titel = "Inglourious Basterds", Erscheinungsjahr = 2009, Spieldauer = 153, Preis = 9.99m, Genre = "War, Drama", FskRating = "FSK 16", Nutzerwertung = 8.3, Handlung = "Jagd auf Nazis.", PosterUrl = "https://image.tmdb.org/t/p/w500/wo2h4p04oo6quuup6o9v6u8ov0s.jpg", Tags = "WW2, Basterds, Tarantino" },
            new Film { Titel = "Joker", Erscheinungsjahr = 2019, Spieldauer = 122, Preis = 12.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.4, Handlung = "Entwicklungsgeschichte eines Bösewichts.", PosterUrl = "https://image.tmdb.org/t/p/w500/bb9797A9Arxt9I7S79UvM9vIayp.jpg", Tags = "Society, Dance, Phoenix" },
            new Film { Titel = "The Usual Suspects", Erscheinungsjahr = 1995, Spieldauer = 106, Preis = 7.99m, Genre = "Crime, Mystery", FskRating = "FSK 12", Nutzerwertung = 8.5, Handlung = "Wer ist Keyser Söze?", PosterUrl = "https://image.tmdb.org/t/p/w500/p25pxoFhYpS28Y7S89v6u8ov0s.jpg", Tags = "Twist, Line-up, Iconic" },
            new Film { Titel = "Alien", Erscheinungsjahr = 1979, Spieldauer = 117, Preis = 8.99m, Genre = "Horror, Sci-Fi", FskRating = "FSK 16", Nutzerwertung = 8.5, Handlung = "Im Weltall hört dich niemand schreien.", PosterUrl = "https://image.tmdb.org/t/p/w500/9gk7p9vS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Space, Monster, Sigourney" },
            new Film { Titel = "Blade Runner", Erscheinungsjahr = 1982, Spieldauer = 117, Preis = 9.99m, Genre = "Sci-Fi, Thriller", FskRating = "FSK 16", Nutzerwertung = 8.1, Handlung = "Jagd auf Replikanten.", PosterUrl = "https://image.tmdb.org/t/p/w500/gEU2Q6Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Cyberpunk, Rain, Vangelis" },
            new Film { Titel = "Saving Private Ryan", Erscheinungsjahr = 1998, Spieldauer = 169, Preis = 9.99m, Genre = "War, Drama", FskRating = "FSK 16", Nutzerwertung = 8.6, Handlung = "Rettung hinter feindlichen Linien.", PosterUrl = "https://image.tmdb.org/t/p/w500/4oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "D-Day, Soldiers, Spielberg" },
            new Film { Titel = "Memento", Erscheinungsjahr = 2000, Spieldauer = 113, Preis = 7.99m, Genre = "Mystery, Thriller", FskRating = "FSK 12", Nutzerwertung = 8.4, Handlung = "Ein Mann ohne Kurzzeitgedächtnis sucht den Mörder seiner Frau.", PosterUrl = "https://image.tmdb.org/t/p/w500/9692vP5LidT0d74lXW4V9dF5hG5.jpg", Tags = "Nonlinear, Tattoos, Nolan" },
            new Film { Titel = "The Departed", Erscheinungsjahr = 2006, Spieldauer = 151, Preis = 9.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.5, Handlung = "Spione in der Mafia und im FBI.", PosterUrl = "https://image.tmdb.org/t/p/w500/6oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Boston, Mafia, Scorsese" },
            new Film { Titel = "Goodfellas", Erscheinungsjahr = 1990, Spieldauer = 145, Preis = 8.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.7, Handlung = "Leben in der Mafia.", PosterUrl = "https://image.tmdb.org/t/p/w500/p25pxoFhYpS28Y7S89v6u8ov0s.jpg", Tags = "Mafia, True Life, Scorsese" },
            new Film { Titel = "The Wolf of Wall Street", Erscheinungsjahr = 2013, Spieldauer = 180, Preis = 11.99m, Genre = "Biography, Crime", FskRating = "FSK 16", Nutzerwertung = 8.2, Handlung = "Aufstieg und Fall von Jordan Belfort.", PosterUrl = "https://image.tmdb.org/t/p/w500/wo2h4p04oo6quuup6o9v6u8ov0s.jpg", Tags = "Money, Drugs, Scorsese" },
            new Film { Titel = "Reservoir Dogs", Erscheinungsjahr = 1992, Spieldauer = 99, Preis = 6.99m, Genre = "Crime, Thriller", FskRating = "FSK 18", Nutzerwertung = 8.3, Handlung = "Ein schiefgelaufener Überfall.", PosterUrl = "https://image.tmdb.org/t/p/w500/3oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Suits, Ear, Tarantino" },
            new Film { Titel = "Once Upon a Time in Hollywood", Erscheinungsjahr = 2019, Spieldauer = 161, Preis = 13.99m, Genre = "Comedy, Drama", FskRating = "FSK 16", Nutzerwertung = 7.6, Handlung = "Hollywood 1969.", PosterUrl = "https://image.tmdb.org/t/p/w500/6oKks7D9e5T6v6t6o9v6u8ov0s.jpg", Tags = "Hollywood, 60s, Tarantino" },
            new Film { Titel = "Heat", Erscheinungsjahr = 1995, Spieldauer = 170, Preis = 9.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.3, Handlung = "Profidieb gegen Top-Polizist.", PosterUrl = "https://image.tmdb.org/t/p/w500/9692vP5LidT0d74lXW4V9dF5hG5.jpg", Tags = "Heist, Pacino, De Niro" },
            new Film { Titel = "Terminator 2: Judgment Day", Erscheinungsjahr = 1991, Spieldauer = 137, Preis = 9.99m, Genre = "Action, Sci-Fi", FskRating = "FSK 16", Nutzerwertung = 8.6, Handlung = "Ein Cyborg soll ein Kind beschützen.", PosterUrl = "https://image.tmdb.org/t/p/w500/gEU2Q6Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Sequel, Tech, Cameron" },
            new Film { Titel = "The Terminator", Erscheinungsjahr = 1984, Spieldauer = 107, Preis = 7.99m, Genre = "Action, Sci-Fi", FskRating = "FSK 16", Nutzerwertung = 8.1, Handlung = "Cyborg-Killer aus der Zukunft.", PosterUrl = "https://image.tmdb.org/t/p/w500/f89U3Y9pS7yV6F8vH9w5Vl0J5s.jpg", Tags = "Cyborg, Survival, Cameron" },
            new Film { Titel = "Braveheart", Erscheinungsjahr = 1995, Spieldauer = 178, Preis = 8.99m, Genre = "Biography, Drama, War", FskRating = "FSK 16", Nutzerwertung = 8.4, Handlung = "Kampf um Schottlands Freiheit.", PosterUrl = "https://image.tmdb.org/t/p/w500/wo2h4p04oo6quuup6o9v6u8ov0s.jpg", Tags = "Freedom, Medieval, Scotland" }
        };

        context.Filme.AddRange(films);
        await context.SaveChangesAsync();

        // --- Relationships (Join-Table) ---
        // I'll use a helper logic to link them based on the list above.
        // For simplicity in this massive seed, I'll assign the corresponding persons to their movies.
        
        var dbFilms = await context.Filme.ToListAsync();
        var dbPersons = await context.Personen.ToListAsync();

        var links = new List<PersonEigenschaftFilm>();

        // Helper to find persons
        Person GetP(string vorname) => dbPersons.First(p => p.Vorname == vorname);
        Film GetF(string titel) => dbFilms.First(f => f.Titel == titel);

        void AddLink(string filmTitel, string personVorname, int rollenId)
        {
            links.Add(new PersonEigenschaftFilm { FilmID = GetF(filmTitel).FilmID, PersonID = GetP(personVorname).PersonID, EigenschaftID = rollenId });
        }

        // Assigning to at least ensure Director, Producer, Lead Actor for many
        
        // Nolan Movies
        AddLink("The Dark Knight", "Christopher", REGIE);
        AddLink("The Dark Knight", "Christian", SCHAUSPIELER);
        AddLink("Inception", "Christopher", REGIE);
        AddLink("Inception", "Leonardo", SCHAUSPIELER);
        AddLink("Interstellar", "Christopher", REGIE);
        AddLink("Interstellar", "Matthew", SCHAUSPIELER);
        AddLink("The Prestige", "Christopher", REGIE);
        AddLink("The Prestige", "Christian", SCHAUSPIELER);
        AddLink("Memento", "Christopher", REGIE);

        // Tarantino Movies
        AddLink("Pulp Fiction", "Quentin", REGIE);
        AddLink("Django Unchained", "Quentin", REGIE);
        AddLink("Django Unchained", "Leonardo", SCHAUSPIELER);
        AddLink("Inglourious Basterds", "Quentin", REGIE);
        AddLink("Inglourious Basterds", "Brad", SCHAUSPIELER);
        AddLink("Reservoir Dogs", "Quentin", REGIE);
        AddLink("Once Upon a Time in Hollywood", "Quentin", REGIE);
        AddLink("Once Upon a Time in Hollywood", "Leonardo", SCHAUSPIELER);
        AddLink("Once Upon a Time in Hollywood", "Brad", SCHAUSPIELER);

        // Scorsese Movies
        AddLink("The Departed", "Martin", REGIE);
        AddLink("The Departed", "Leonardo", SCHAUSPIELER);
        AddLink("Goodfellas", "Martin", REGIE);
        AddLink("Goodfellas", "Robert", SCHAUSPIELER);
        AddLink("The Wolf of Wall Street", "Martin", REGIE);
        AddLink("The Wolf of Wall Street", "Leonardo", SCHAUSPIELER);

        // Spielberg Movies
        AddLink("Schindler's List", "Steven", REGIE);
        AddLink("Jurassic Park", "Steven", REGIE);
        AddLink("Saving Private Ryan", "Steven", REGIE);
        AddLink("Saving Private Ryan", "Tom", SCHAUSPIELER);
        AddLink("Forrest Gump", "Tom", SCHAUSPIELER);

        // Cameron Movies
        AddLink("Avatar", "James", REGIE);
        AddLink("Titanic", "James", REGIE);
        AddLink("Titanic", "Leonardo", SCHAUSPIELER);
        AddLink("Terminator 2: Judgment Day", "James", REGIE);
        AddLink("The Terminator", "James", REGIE);

        // Scott Movies
        AddLink("Gladiator", "Ridley", REGIE);
        AddLink("Alien", "Ridley", REGIE);
        AddLink("Blade Runner", "Ridley", REGIE);

        // Fincher Movies
        AddLink("Fight Club", "David", REGIE);
        AddLink("Fight Club", "Brad", SCHAUSPIELER);
        AddLink("Se7en", "David", REGIE);
        AddLink("Se7en", "Brad", SCHAUSPIELER);

        // Others
        AddLink("Matrix", "Keanu", SCHAUSPIELER);
        AddLink("The Shawshank Redemption", "Morgan", SCHAUSPIELER);
        AddLink("The Godfather", "Francis Ford", REGIE);
        AddLink("The Godfather", "Al", SCHAUSPIELER);
        AddLink("The Godfather", "Robert", SCHAUSPIELER);
        AddLink("Heat", "Al", SCHAUSPIELER);
        AddLink("Heat", "Robert", SCHAUSPIELER);
        AddLink("Joker", "Robert", SCHAUSPIELER);

        context.PersonEigenschaftFilme.AddRange(links);
        await context.SaveChangesAsync();
    }
}
