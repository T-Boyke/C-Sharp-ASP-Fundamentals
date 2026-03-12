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
            new Person { Vorname = "Christopher", Nachname = "Nolan", Biografie = "Christopher Edward Nolan CBE (* 30. Juli 1970 in London) ist ein britisch-US-amerikanischer Filmregisseur, Drehbuchautor und Filmproduzent. Bekannt für Meisterwerke wie Inception, Interstellar und The Dark Knight.", Geburtsdatum = new DateTime(1970, 7, 30), Geburtsort = "Westminster, London, UK", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/xuAIuYSmsUzKlUMBFGVZaWsY3DZ.jpg", Tags = "Genius, Nonlinear, IMAX" },
            new Person { Vorname = "Quentin", Nachname = "Tarantino", Biografie = "Quentin Jerome Tarantino (* 27. März 1963 in Knoxville, Tennessee) ist ein US-amerikanischer Regisseur, Schauspieler und Drehbuchautor, der für seine stilisierten Gewaltszenen und cleveren Dialoge bekannt ist.", Geburtsdatum = new DateTime(1963, 3, 27), Geburtsort = "Knoxville, Tennessee, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/1gjcpAa99FAOWGnrUvHEXXsRs7o.jpg", Tags = "Dialogue, Violence, Feet" },
            new Person { Vorname = "Steven", Nachname = "Spielberg", Biografie = "Steven Allan Spielberg (* 18. Dezember 1946 in Cincinnati, Ohio) ist ein US-amerikanischer Regisseur, Produzent und Drehbuchautor. Er gilt als einer der einflussreichsten Filmemacher aller Zeiten.", Geburtsdatum = new DateTime(1946, 12, 18), Geburtsort = "Cincinnati, Ohio, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/tZxcg19YQ3e8fJ0pOs7hjlnmmr6.jpg", Tags = "Blockbuster, Amblin, Legend" },
            new Person { Vorname = "Martin", Nachname = "Scorsese", Biografie = "Martin Charles Scorsese (* 17. November 1942 in Queens, New York City) ist ein US-amerikanischer Regisseur, Drehbuchautor und Filmproduzent. Bekannt für Klassiker wie Taxi Driver und Goodfellas.", Geburtsdatum = new DateTime(1942, 11, 17), Geburtsort = "New York City, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/9U9Y5GQuWX3EZy39B8nkk4NY01S.jpg", Tags = "Crime, Catholic, Legend" },
            new Person { Vorname = "James", Nachname = "Cameron", Biografie = "James Francis Cameron (* 16. August 1954 in Kapuskasing, Ontario) ist ein kanadischer Regisseur, Produzent und Tiefseeforscher. Schöpfer von Terminator, Aliens, Titanic und Avatar.", Geburtsdatum = new DateTime(1954, 8, 16), Geburtsort = "Kapuskasing, Ontario, Kanada", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/9NAZnTjBQ9WcXAQEzZpKy4vdQto.jpg", Tags = "Sci-Fi, Tech, Ocean" },
            new Person { Vorname = "Ridley", Nachname = "Scott", Biografie = "Sir Ridley Scott (* 30. November 1937 in South Shields) ist ein britischer Regisseur und Produzent. Bekannt für Alien, Blade Runner und Gladiator.", Geburtsdatum = new DateTime(1937, 11, 30), Geburtsort = "South Shields, Tyndall, UK", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/zABJmN9opmqD4orWl3KSdCaSo7Q.jpg", Tags = "Sci-Fi, Visuals, Historical" },
            new Person { Vorname = "David", Nachname = "Fincher", Biografie = "David Andrew Leo Fincher (* 28. August 1962 in Denver, Colorado) ist ein US-amerikanischer Regisseur und Produzent. Meister des atmosphärischen Thrillers (Se7en, Fight Club).", Geburtsdatum = new DateTime(1962, 8, 28), Geburtsort = "Denver, Colorado, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/tpEczFclQZeKAiCeKZZ0adRvtfz.jpg", Tags = "Perfectionist, Dark, Digital" },
            new Person { Vorname = "Francis Ford", Nachname = "Coppola", Biografie = "Francis Ford Coppola (* 7. April 1939 in Detroit, Michigan) ist ein US-amerikanischer Regisseur und Produzent. Er schuf mit Der Pate (The Godfather) einen der bedeutendsten Filme der Geschichte.", Geburtsdatum = new DateTime(1939, 4, 7), Geburtsort = "Detroit, Michigan, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/IwGgkmW6IoJ9vuNF0T9CU3FYUX.jpg", Tags = "New Hollywood, Legend" },
            
            // Actors
            new Person { Vorname = "Leonardo", Nachname = "DiCaprio", Biografie = "Leonardo Wilhelm DiCaprio (* 11. November 1974 in Los Angeles) ist ein US-amerikanischer Schauspieler, Filmproduzent und Umweltaktivist. Oscar-Gewinner für The Revenant.", Geburtsdatum = new DateTime(1974, 11, 11), Geburtsort = "Los Angeles, California, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/vo4fltT9zZ1kH8nhLetz8MED6jp.jpg", Tags = "Method, Climate, Legend" },
            new Person { Vorname = "Brad", Nachname = "Pitt", Biografie = "William Bradley Pitt (* 18. Dezember 1963 in Shawnee, Oklahoma) ist ein US-amerikanischer Schauspieler und Filmproduzent. Bekannt für Fight Club, Se7en und Ocean's Eleven.", Geburtsdatum = new DateTime(1963, 12, 18), Geburtsort = "Shawnee, Oklahoma, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/cckcYc2v0yh1tc9QjRelptcOBko.jpg", Tags = "Charismatic, Producer" },
            new Person { Vorname = "Tom", Nachname = "Hanks", Biografie = "Thomas Jeffrey Hanks (* 9. Juli 1956 in Concord, Kalifornien) ist einer der profiliertesten Charakterdarsteller Hollywoods. Bekannt für Forrest Gump und Philadelphia.", Geburtsdatum = new DateTime(1956, 7, 9), Geburtsort = "Concord, California, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/oFvZoKI6lvU03n4YoNGAll9rkas.jpg", Tags = "Nicest, Everyman" },
            new Person { Vorname = "Morgan", Nachname = "Freeman", Biografie = "Morgan Freeman (* 1. Juni 1937 in Memphis, Tennessee) ist ein US-amerikanischer Schauspieler und Produzent. Bekannt für seine markante Stimme und Rollen in Die Verurteilten.", Geburtsdatum = new DateTime(1937, 6, 1), Geburtsort = "Memphis, Tennessee, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/jPsLqiYGSofU4s6BjrxnefMfabb.jpg", Tags = "Voice, Legend" },
            new Person { Vorname = "Christian", Nachname = "Bale", Biografie = "Christian Charles Philip Bale (* 30. Januar 1974 in Haverfordwest) ist ein britischer Schauspieler. Bekannt für seine intensiven Körpertransformationen und als Batman.", Geburtsdatum = new DateTime(1974, 1, 30), Geburtsort = "Haverfordwest, Pembrokeshire, UK", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/7Pxez9J8fuPd2Mn9kex13YALrCQ.jpg", Tags = "Transformation, Intensity" },
            new Person { Vorname = "Matthew", Nachname = "McConaughey", Biografie = "Matthew David McConaughey (* 4. November 1969 in Uvalde, Texas) ist ein US-amerikanischer Schauspieler und Oscar-Gewinner für Dallas Buyers Club.", Geburtsdatum = new DateTime(1969, 11, 4), Geburtsort = "Uvalde, Texas, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/lCySuYjhXix3FzQdS4oceDDrXKI.jpg", Tags = "Alright, McConaissance" },
            new Person { Vorname = "Keanu", Nachname = "Reeves", Biografie = "Keanu Charles Reeves (* 2. September 1964 in Beirut, Libanon) ist ein kanadischer Schauspieler und Musiker. Weltberühmt durch Matrix und John Wick.", Geburtsdatum = new DateTime(1964, 9, 2), Geburtsort = "Beirut, Libanon", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/8RZLOyYGsoRe9p44q3xin9QkMHv.jpg", Tags = "Wholesome, Cyberpunk" },
            new Person { Vorname = "Robert", Nachname = "De Niro", Biografie = "Robert Anthony De Niro Jr. (* 17. August 1943 in New York City) gilt als einer der besten Charakterdarsteller der Filmgeschichte. Bekannt für Taxi Driver und Wie ein wilder Stier.", Geburtsdatum = new DateTime(1943, 8, 17), Geburtsort = "New York City, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/cT8htccfvNpYp71u9sly17u697G.jpg", Tags = "Method, Gangster" },
            new Person { Vorname = "Al", Nachname = "Pacino", Biografie = "Alfredo James Pacino (* 25. April 1940 in East Harlem) ist ein US-amerikanischer Schauspieler. Berühmt für seine Rollen als Michael Corleone und Oscar-Gewinner für Der Duft der Frauen.", Geburtsdatum = new DateTime(1940, 4, 25), Geburtsort = "New York City, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/m8HAAjq1T75JypKk0v1FFQn4ysZ.jpg", Tags = "Intensity, Legend" },
            new Person { Vorname = "Emma", Nachname = "Thomas", Biografie = "Emma Thomas (* 9. Dezember 1971 in London) ist eine britische Filmproduzentin und Vizepräsidentin der Syncopy Inc. Sie ist die Ehefrau von Christopher Nolan.", Geburtsdatum = new DateTime(1971, 12, 9), Geburtsort = "London, UK", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/utc1PS6WVWR5tknzTJqXtnD0kBp.jpg", Tags = "Producer, Syncopy" },
            new Person { Vorname = "Mel", Nachname = "Gibson", Biografie = "Mel Columcille Gerard Gibson AO (* 3. Januar 1956 in Peekskill, New York) ist ein US-amerikanischer Schauspieler, Regisseur und Produzent. Oscar-Gewinner für Braveheart.", Geburtsdatum = new DateTime(1956, 1, 3), Geburtsort = "Peekskill, New York, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/jnqHMaOslt8cef2atSmOpGRvNla.jpg", Tags = "Action, Director, Legend" },
            new Person { Vorname = "Kevin", Nachname = "Spacey", Biografie = "Kevin Spacey Fowler KBE (* 26. Juli 1959 in South Orange, New Jersey) ist ein US-amerikanischer Schauspieler und zweifacher Oscar-Preisträger.", Geburtsdatum = new DateTime(1959, 7, 26), Geburtsort = "South Orange, New Jersey, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/nPrUZDEbGQe6jwpVbHKJCXsMd7r.jpg", Tags = "Method, Award-winner" },
            new Person { Vorname = "Bryan", Nachname = "Singer", Biografie = "Bryan Jay Singer (* 17. September 1965) ist ein US-amerikanischer Regisseur und Produzent, bekannt für The Usual Suspects und die X-Men-Reihe.", Geburtsdatum = new DateTime(1965, 9, 17), Geburtsort = "New York City, USA", ProfilBildUrl = "https://image.tmdb.org/t/p/w500/elF6ldrDqgWCYMRYD5uy2cb8Ys0.jpg", Tags = "Director, Sci-Fi" }
        };

        context.Personen.AddRange(persons);
        await context.SaveChangesAsync();

        // --- Films Expansion (30+) ---
        var films = new List<Film>
        {
            new Film { Titel = "Matrix", Erscheinungsjahr = 1999, Spieldauer = 136, Preis = 9.99m, Genre = "Action, Sci-Fi", FskRating = "FSK 16", Nutzerwertung = 8.2, Tagline = "Glaube das Unglaubliche.", Handlung = "Der Hacker Neo wird übers Internet von einer geheimnisvollen Untergrund-Organisation kontaktiert. Er erfährt die bittere Wahrheit über die Matrix.", PosterUrl = "https://image.tmdb.org/t/p/w500/iVmDLujHcV1zaMnaahKWn4TcCS6.jpg", Tags = "Cult, Simulation, Slow-Motion" },
            new Film { Titel = "Inception", Erscheinungsjahr = 2010, Spieldauer = 148, Preis = 12.99m, Genre = "Sci-Fi, Thriller", FskRating = "FSK 12", Nutzerwertung = 8.8, Tagline = "Dein Verstand ist der Ort des Verbrechens.", Handlung = "Ein Dieb, der Geheimnisse aus Träumen stiehlt, bekommt die Chance auf eine saubere Weste, wenn er das Unmögliche schafft: Inception.", PosterUrl = "https://image.tmdb.org/t/p/w500/xlaY2zyzMfkh0HSC5VUwzoZPU1.jpg", Tags = "Dreams, Mind-bending, Hans Zimmer" },

            new Film { Titel = "Interstellar", Erscheinungsjahr = 2014, Spieldauer = 169, Preis = 14.99m, Genre = "Abenteuer, Sci-Fi", FskRating = "FSK 12", Nutzerwertung = 8.7, Tagline = "Die Menschheit wurde auf der Erde geboren. Sie war nie dazu bestimmt, dort zu sterben.", Handlung = "Reise durch ein Wurmloch zur Rettung der Menschheit.", PosterUrl = "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg", Tags = "Space, Wormhole, Tears" },
            new Film { Titel = "The Dark Knight", Erscheinungsjahr = 2008, Spieldauer = 152, Preis = 9.99m, Genre = "Action, Krimi", FskRating = "FSK 16", Nutzerwertung = 8.5, Tagline = "Warum so ernst?", Handlung = "Batman kämpft gegen den Joker, einen psychotischen Superverbrecher, der Gotham in Chaos stürzt.", PosterUrl = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg", Tags = "Joker, Masterpiece, Batman" },
            new Film { Titel = "Pulp Fiction", Erscheinungsjahr = 1994, Spieldauer = 154, Preis = 7.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.9, Tagline = "Du wirst nicht wissen, was dich getroffen hat.", Handlung = "Verflochtene Geschichten in Los Angeles.", PosterUrl = "https://image.tmdb.org/t/p/w500/vQWk5YBFWF4bZaofAbv0tShwBvQ.jpg", Tags = "Dialogue, Iconic, Nonlinear" },
            new Film { Titel = "Fight Club", Erscheinungsjahr = 1999, Spieldauer = 139, Preis = 8.99m, Genre = "Drama, Thriller", FskRating = "FSK 18", Nutzerwertung = 8.8, Tagline = "Verliere alles, um alles zu gewinnen.", Handlung = "Ein unter Schlaflosigkeit leidender Mann gründet einen Boxclub.", PosterUrl = "https://image.tmdb.org/t/p/w500/pB8BM7pdSp6B6Ih7QZ4DrQ3PmJK.jpg", Tags = "Chaos, Twist, Soap" },
            new Film { Titel = "Forrest Gump", Erscheinungsjahr = 1994, Spieldauer = 142, Preis = 9.99m, Genre = "Drama, Romance", FskRating = "FSK 12", Nutzerwertung = 8.8, Tagline = "Die Welt wird nie mehr dieselbe sein, wenn man sie erst einmal durch die Augen von Forrest Gump gesehen hat.", Handlung = "Das außergewöhnliche Leben eines einfachen Mannes.", PosterUrl = "https://image.tmdb.org/t/p/w500/arw2vcBveWOvVpBaseKqh26cgRh.jpg", Tags = "History, Running, Chocolate" },
            new Film { Titel = "The Shawshank Redemption", Erscheinungsjahr = 1994, Spieldauer = 142, Preis = 9.99m, Genre = "Drama, Crime", FskRating = "FSK 12", Nutzerwertung = 9.3, Tagline = "Angst kann dich gefangen halten. Hoffnung kann dich frei machen.", Handlung = "Hoffnung hinter Gefängnismauern.", PosterUrl = "https://image.tmdb.org/t/p/w500/lypGstPsB7n9DIr0Gmyam97q2nd.jpg", Tags = "Hope, Freedom, King" },
            new Film { Titel = "The Godfather", Erscheinungsjahr = 1972, Spieldauer = 175, Preis = 14.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 9.2, Tagline = "Ein Angebot, das man nicht ablehnen kann.", Handlung = "Der Aufstieg einer Mafia-Familie.", PosterUrl = "https://image.tmdb.org/t/p/w500/3bhkrj58Vtu7enYsRolD1fZdja1.jpg", Tags = "Mafia, Family, Masterpiece" },
            new Film { Titel = "Schindler's List", Erscheinungsjahr = 1993, Spieldauer = 195, Preis = 12.99m, Genre = "War, Drama", FskRating = "FSK 12", Nutzerwertung = 9.0, Tagline = "Wer nur ein einziges Leben rettet, rettet die ganze Welt.", Handlung = "Rettung während des Holocausts.", PosterUrl = "https://image.tmdb.org/t/p/w500/sF1U4EUQS8YHUYjNl3pMGNIQyr0.jpg", Tags = "Holocaust, Black and White, Moving" },
            new Film { Titel = "Gladiator", Erscheinungsjahr = 2000, Spieldauer = 155, Preis = 9.99m, Genre = "Action, Adventure", FskRating = "FSK 16", Nutzerwertung = 8.5, Tagline = "Ein General, der zum Sklaven wurde. Ein Sklave, der zum Gladiator wurde. Ein Gladiator, der einem Kaiser trotzte.", Handlung = "Ein verratener General wird Gladiator.", PosterUrl = "https://image.tmdb.org/t/p/w500/ty8TGRuvJLPUmAR1H1nRIsgwvim.jpg", Tags = "Rome, Revange, Epic" },
            new Film { Titel = "Avatar", Erscheinungsjahr = 2009, Spieldauer = 162, Preis = 12.99m, Genre = "Adventure, Sci-Fi", FskRating = "FSK 12", Nutzerwertung = 7.9, Tagline = "Betrete die Welt.", Handlung = "Kampf auf Pandora.", PosterUrl = "https://image.tmdb.org/t/p/w500/gKY6q7SjCkAU6FqvqWybDYgUKIF.jpg", Tags = "3D, Visuals, Pandora" },
            new Film { Titel = "Titanic", Erscheinungsjahr = 1997, Spieldauer = 194, Preis = 9.99m, Genre = "Drama, Romance", FskRating = "FSK 12", Nutzerwertung = 7.9, Tagline = "Nichts auf der Welt konnte sie trennen.", Handlung = "Liebe auf dem sinkenden Schiff.", PosterUrl = "https://image.tmdb.org/t/p/w500/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg", Tags = "Iceberg, Celine Dion, Epic" },
            new Film { Titel = "Jurassic Park", Erscheinungsjahr = 1993, Spieldauer = 127, Preis = 8.99m, Genre = "Adventure, Sci-Fi", FskRating = "FSK 12", Nutzerwertung = 8.2, Tagline = "Es hat 65 Millionen Jahre gedauert.", Handlung = "Dinosaurier kehren zurück.", PosterUrl = "https://image.tmdb.org/t/p/w500/maFjKnJ62hDQ9E66dKqDZgbUy0H.jpg", Tags = "Dinosaurs, Theme Park, Spielberg" },
            new Film { Titel = "Se7en", Erscheinungsjahr = 1995, Spieldauer = 127, Preis = 8.99m, Genre = "Crime, Thriller", FskRating = "FSK 16", Nutzerwertung = 8.6, Tagline = "Sieben Sünden. Sieben Opfer.", Handlung = "Suche nach einem Serienmörder.", PosterUrl = "https://image.tmdb.org/t/p/w500/191nKfP0ehp3uIvWqgPbFmI4lv9.jpg", Tags = "Seven Sins, Dark, Fincher" },
            new Film { Titel = "The Prestige", Erscheinungsjahr = 2006, Spieldauer = 130, Preis = 10.99m, Genre = "Drama, Mystery", FskRating = "FSK 12", Nutzerwertung = 8.5, Tagline = "Bist du aufmerksam?", Handlung = "Rivalisierende Magier.", PosterUrl = "https://image.tmdb.org/t/p/w500/Ag2B2KHKQPukjH7WutmgnnSNurZ.jpg", Tags = "Magic, Rivalry, Nolan" },
            new Film { Titel = "Django Unchained", Erscheinungsjahr = 2012, Spieldauer = 165, Preis = 11.99m, Genre = "Western, Drama", FskRating = "FSK 16", Nutzerwertung = 8.4, Tagline = "Das 'D' ist stumm.", Handlung = "Ein Sklave wird Kopfgeldjäger.", PosterUrl = "https://image.tmdb.org/t/p/w500/7oWY8VDWW7thTzWh3OKYRkWUlD5.jpg", Tags = "Western, Django, Tarantino" },
            new Film { Titel = "Inglourious Basterds", Erscheinungsjahr = 2009, Spieldauer = 153, Preis = 9.99m, Genre = "War, Drama", FskRating = "FSK 16", Nutzerwertung = 8.3, Tagline = "Es war einmal in einem von Nazis besetzten Frankreich...", Handlung = "Jagd auf Nazis.", PosterUrl = "https://image.tmdb.org/t/p/w500/7sfbEnaARXDDhKm0CZ7D7uc2sbo.jpg", Tags = "WW2, Basterds, Tarantino" },
            new Film { Titel = "Joker", Erscheinungsjahr = 2019, Spieldauer = 122, Preis = 12.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.4, Tagline = "Setz ein glückliches Gesicht auf.", Handlung = "Entwicklungsgeschichte eines Bösewichts.", PosterUrl = "https://image.tmdb.org/t/p/w500/udDclJoHjfjb8Ekgsd4FDteOkCU.jpg", Tags = "Society, Dance, Phoenix" },
            new Film { Titel = "The Usual Suspects", Erscheinungsjahr = 1995, Spieldauer = 106, Preis = 7.99m, Genre = "Crime, Mystery", FskRating = "FSK 12", Nutzerwertung = 8.5, Tagline = "Die Wahrheit ist manchmal schwer zu finden.", Handlung = "Wer ist Keyser Söze?", PosterUrl = "https://image.tmdb.org/t/p/w500/99X2SgyFunJFXGAYnDv3sb9pnUD.jpg", Tags = "Twist, Line-up, Iconic" },
            new Film { Titel = "Alien", Erscheinungsjahr = 1979, Spieldauer = 117, Preis = 8.99m, Genre = "Horror, Sci-Fi", FskRating = "FSK 16", Nutzerwertung = 8.5, Tagline = "Im Weltraum hört dich niemand schreien.", Handlung = "Im Weltall hört dich niemand schreien.", PosterUrl = "https://image.tmdb.org/t/p/w500/vfrQk5IPloGg1v9Rzbh2Eg3VGyM.jpg", Tags = "Space, Monster, Sigourney" },
            new Film { Titel = "Blade Runner", Erscheinungsjahr = 1982, Spieldauer = 117, Preis = 9.99m, Genre = "Sci-Fi, Thriller", FskRating = "FSK 16", Nutzerwertung = 8.1, Tagline = "Ich habe Dinge gesehen, die ihr Menschen niemals glauben würdet.", Handlung = "Jagd auf Replikanten.", PosterUrl = "https://image.tmdb.org/t/p/w500/63N9uy8nd9j7Eog2axPQ8lbr3Wj.jpg", Tags = "Cyberpunk, Rain, Vangelis" },
            new Film { Titel = "Saving Private Ryan", Erscheinungsjahr = 1998, Spieldauer = 169, Preis = 9.99m, Genre = "War, Drama", FskRating = "FSK 16", Nutzerwertung = 8.6, Tagline = "Die Mission ist ein Mann.", Handlung = "Rettung hinter feindlichen Linien.", PosterUrl = "https://image.tmdb.org/t/p/w500/uqx37cS8cpHg8U35f9U5IBlrCV3.jpg", Tags = "D-Day, Soldiers, Spielberg" },
            new Film { Titel = "Memento", Erscheinungsjahr = 2000, Spieldauer = 113, Preis = 7.99m, Genre = "Mystery, Thriller", FskRating = "FSK 12", Nutzerwertung = 8.4, Tagline = "Manche Dinge vergisst man besser.", Handlung = "Ein Mann ohne Kurzzeitgedächtnis sucht den Mörder seiner Frau.", PosterUrl = "https://image.tmdb.org/t/p/w500/fKTPH2WvH8nHTXeBYBVhawtRqtR.jpg", Tags = "Nonlinear, Tattoos, Nolan" },
            new Film { Titel = "The Departed", Erscheinungsjahr = 2006, Spieldauer = 151, Preis = 9.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.5, Tagline = "Lügst du noch oder stirbst du schon?", Handlung = "Spione in der Mafia und im FBI.", PosterUrl = "https://image.tmdb.org/t/p/w500/nT97ifVT2J1yMQmeq20Qblg61T.jpg", Tags = "Boston, Mafia, Scorsese" },
            new Film { Titel = "Goodfellas", Erscheinungsjahr = 1990, Spieldauer = 145, Preis = 8.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.7, Tagline = "Drei Jahrzehnte Leben in der Mafia.", Handlung = "Leben in der Mafia.", PosterUrl = "https://image.tmdb.org/t/p/w500/9OkCLM73MIU2CrKZbqiT8Ln1wY2.jpg", Tags = "Mafia, True Life, Scorsese" },
            new Film { Titel = "The Wolf of Wall Street", Erscheinungsjahr = 2013, Spieldauer = 180, Preis = 11.99m, Genre = "Biography, Crime", FskRating = "FSK 16", Nutzerwertung = 8.2, Tagline = "Geld. Macht. Frauen. Drogen.", Handlung = "Aufstieg und Fall von Jordan Belfort.", PosterUrl = "https://image.tmdb.org/t/p/w500/kW9LmvYHAaS9iA0tHmZVq8hQYoq.jpg", Tags = "Money, Drugs, Scorsese" },
            new Film { Titel = "Reservoir Dogs", Erscheinungsjahr = 1992, Spieldauer = 99, Preis = 6.99m, Genre = "Crime, Thriller", FskRating = "FSK 18", Nutzerwertung = 8.3, Tagline = "Jeder stirbt. Einer singt.", Handlung = "Ein schiefgelaufener Überfall.", PosterUrl = "https://image.tmdb.org/t/p/w500/xi8Iu6qyTfyZVDVy60raIOYJJmk.jpg", Tags = "Suits, Ear, Tarantino" },
            new Film { Titel = "Once Upon a Time in Hollywood", Erscheinungsjahr = 2019, Spieldauer = 161, Preis = 13.99m, Genre = "Comedy, Drama", FskRating = "FSK 16", Nutzerwertung = 7.6, Tagline = "Es war einmal in Hollywood.", Handlung = "Hollywood 1969.", PosterUrl = "https://image.tmdb.org/t/p/w500/8j58iEBw9pOXFD2L0nt0ZXeHviB.jpg", Tags = "Hollywood, 60s, Tarantino" },
            new Film { Titel = "Heat", Erscheinungsjahr = 1995, Spieldauer = 170, Preis = 9.99m, Genre = "Crime, Drama", FskRating = "FSK 16", Nutzerwertung = 8.3, Tagline = "Ein Profi-Dieb. Ein Top-Polizist. Das ultimative Duell.", Handlung = "Profidieb gegen Top-Polizist.", PosterUrl = "https://image.tmdb.org/t/p/w500/e09dLw1Ljtccd2P4NsuUvVtS5du.jpg", Tags = "Heist, Pacino, De Niro" },
            new Film { Titel = "Terminator 2: Judgment Day", Erscheinungsjahr = 1991, Spieldauer = 137, Preis = 9.99m, Genre = "Action, Sci-Fi", FskRating = "FSK 16", Nutzerwertung = 8.6, Tagline = "Tag der Abrechnung.", Handlung = "Ein Cyborg soll ein Kind beschützen.", PosterUrl = "https://image.tmdb.org/t/p/w500/jFTVD4XoWQTcg7wdyJKa8PEds5q.jpg", Tags = "Sequel, Tech, Cameron" },
            new Film { Titel = "The Terminator", Erscheinungsjahr = 1984, Spieldauer = 107, Preis = 7.99m, Genre = "Action, Sci-Fi", FskRating = "FSK 16", Nutzerwertung = 8.1, Tagline = "Nichts wird jemals wieder dasselbe sein.", Handlung = "Cyborg-Killer aus der Zukunft.", PosterUrl = "https://image.tmdb.org/t/p/w500/qvktm0BHcnmDpul4Hz01GIazWPr.jpg", Tags = "Cyborg, Survival, Cameron" },
            new Film { Titel = "Braveheart", Erscheinungsjahr = 1995, Spieldauer = 178, Preis = 8.99m, Genre = "Biography, Drama, War", FskRating = "FSK 16", Nutzerwertung = 8.4, Tagline = "Jeder Mann stirbt - nicht jeder Mann lebt wirklich.", Handlung = "Kampf um Schottlands Freiheit.", PosterUrl = "https://image.tmdb.org/t/p/w500/or1gBugydmjToAEq7OZY0owwFk.jpg", Tags = "Freedom, Medieval, Scotland" }
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
        AddLink("The Dark Knight", "Emma", PRODUZENT);
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
        AddLink("Braveheart", "Mel", REGIE);
        AddLink("Braveheart", "Mel", SCHAUSPIELER);
        AddLink("The Usual Suspects", "Bryan", REGIE);
        AddLink("The Usual Suspects", "Kevin", SCHAUSPIELER);

        context.PersonEigenschaftFilme.AddRange(links);
        await context.SaveChangesAsync();
    }
}
