using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DebugSearch
{
    public static async Task Main()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("DebugDb")
            .Options;

        using var context = new ApplicationDbContext(options);

        // Seed
        var nolan = new Person { Vorname = "Christopher", Nachname = "Nolan" };
        context.Personen.Add(nolan);
        
        var inception = new Film { Titel = "Inception", Erscheinungsjahr = 2010, Preis = 9.99m };
        context.Filme.Add(inception);
        await context.SaveChangesAsync();

        var directorTag = new Eigenschaft { Bezeichnung = "Director" };
        context.Eigenschaften.Add(directorTag);
        await context.SaveChangesAsync();

        context.PersonEigenschaftFilme.Add(new PersonEigenschaftFilm
        {
            Film = inception,
            Person = nolan,
            Eigenschaft = directorTag
        });
        await context.SaveChangesAsync();

        // Search logic from controller
        string searchString = "Christopher Nolan";
        var searchTerms = searchString.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        var query = context.Filme
            .Include(f => f.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Person)
            .AsQueryable();

        foreach (var term in searchTerms)
        {
            query = query.Where(f => f.Titel.ToLower().Contains(term) 
                                || (f.Handlung != null && f.Handlung.ToLower().Contains(term))
                                || f.PersonEigenschaftFilme.Any(pef => pef.Person != null && 
                                    ((pef.Person.Vorname != null && pef.Person.Vorname.ToLower().Contains(term)) || 
                                     (pef.Person.Nachname != null && pef.Person.Nachname.ToLower().Contains(term)))));
        }

        var results = await query.ToListAsync();

        Console.WriteLine($"Search terms: {string.Join(", ", searchTerms)}");
        Console.WriteLine($"Results found: {results.Count}");
        foreach (var r in results)
        {
            Console.WriteLine($"- {r.Titel}");
        }
        
        if (results.Count == 0)
        {
            // Debugging why it fails
            var allMovies = await context.Filme.Include(f => f.PersonEigenschaftFilme).ThenInclude(pef => pef.Person).ToListAsync();
            foreach (var m in allMovies)
            {
                Console.WriteLine($"Movie: {m.Titel}");
                Console.WriteLine($"  PEF count: {m.PersonEigenschaftFilme.Count}");
                foreach (var pef in m.PersonEigenschaftFilme)
                {
                    Console.WriteLine($"    Person: {pef.Person?.Vorname} {pef.Person?.Nachname}");
                    if (pef.Person != null)
                    {
                        Console.WriteLine($"      Vorname match 'christopher': {pef.Person.Vorname.ToLower().Contains("christopher")}");
                        Console.WriteLine($"      Nachname match 'nolan': {pef.Person.Nachname.ToLower().Contains("nolan")}");
                    }
                }
            }
        }
    }
}
