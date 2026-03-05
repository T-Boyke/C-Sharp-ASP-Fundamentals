using Microsoft.EntityFrameworkCore;
using _06_NewsApplication2.Domain.Entities;

namespace _06_NewsApplication2.Infrastructure.Data;

/// <summary>
/// Hilfsklasse zur Sicherstellung der Datenbankexistenz und für Seed-Daten.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Authors.AnyAsync()) return;

        var authors = new List<Author>
        {
            new Author { Firstname = "Max", Lastname = "Mustermann" },
            new Author { Firstname = "Erika", Lastname = "Musterfrau" }
        };

        await context.Authors.AddRangeAsync(authors);
        await context.SaveChangesAsync();

        var articles = new List<Article>
        {
            new Article 
            { 
                Headline = "Willkommen bei NewsApplication2", 
                Content = "Dies ist ein Beispielartikel, der die asynchrone Programmierung in ASP.NET Core 10 demonstriert. Wir nutzen DDD und Tailwind CSS.", 
                AuthorId = authors[0].Id,
                CreatedAt = DateTime.Now
            },
            new Article 
            { 
                Headline = "Modernes Webdesign mit Tailwind 4", 
                Content = "Tailwind CSS 4.1.13 bietet unglaubliche neue Features für performante User Interfaces. In dieser App nutzen wir es für alle Komponenten.", 
                AuthorId = authors[1].Id,
                CreatedAt = DateTime.Now.AddHours(-2)
            }
        };

        await context.Articles.AddRangeAsync(articles);
        await context.SaveChangesAsync();
    }
}
