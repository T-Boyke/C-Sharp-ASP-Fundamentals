using System;
using System.Linq;
using System.Threading.Tasks;
using _10_Filmdatenbank.Domain.Entities;
using Bogus;
using Microsoft.EntityFrameworkCore;

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

            var personFaker = new Faker<_10_Filmdatenbank.Domain.Entities.Person>()
                .RuleFor(p => p.Vorname, (f, p) => f.Name.FirstName())
                .RuleFor(p => p.Nachname, (f, p) => f.Name.LastName());

            var persons = personFaker.Generate(50);
            context.Personen.AddRange(persons);
            await context.SaveChangesAsync();

            var filmFaker = new Faker<Film>()
                .RuleFor(f => f.Titel, (f, flm) => f.Lorem.Sentence(3)) 
                .RuleFor(f => f.Erscheinungsjahr, (f, flm) => f.Date.Past(30).Year)
                .RuleFor(f => f.Spieldauer, (f, flm) => f.Random.Int(80, 180))
                .RuleFor(f => f.Preis, (f, flm) => f.Random.Decimal(9.99m, 29.99m));

            // Specialized titles for some
            var filmTitles = new[] { "Matrix", "High Noon", "Inception", "The Dark Knight", "Pulp Fiction" };
            var films = filmFaker.Generate(45);
            foreach (var title in filmTitles)
            {
                films.Add(new Film { Titel = title, Erscheinungsjahr = 2000, Spieldauer = 120, Preis = 19.99m });
            }

            context.Filme.AddRange(films);
            await context.SaveChangesAsync();

            // Link them randomly
            var targetPersons = await context.Personen.ToListAsync();
            var targetFilms = await context.Filme.ToListAsync();
            var targetEigenschaften = await context.Eigenschaften.ToListAsync();

            var random = new Random();
            var links = new List<PersonEigenschaftFilm>();

            foreach (var film in targetFilms)
            {
                // Assign 1-3 persons per film with random roles
                int count = random.Next(1, 4);
                for (int i = 0; i < count; i++)
                {
                    links.Add(new PersonEigenschaftFilm
                    {
                        FilmID = film.FilmID,
                        PersonID = targetPersons[random.Next(targetPersons.Count)].PersonID,
                        EigenschaftID = targetEigenschaften[random.Next(targetEigenschaften.Count)].EigenschaftID
                    });
                }
            }

        context.PersonEigenschaftFilme.AddRange(links);
        await context.SaveChangesAsync();
    }
}
