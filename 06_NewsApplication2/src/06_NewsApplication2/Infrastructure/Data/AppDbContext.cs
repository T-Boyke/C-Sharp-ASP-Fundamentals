using Microsoft.EntityFrameworkCore;
using _06_NewsApplication2.Domain.Entities;

namespace _06_NewsApplication2.Infrastructure.Data;

/// <summary>
/// Datenbankkontext für die News-Anwendung.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguration für Article
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasOne(a => a.Author)
                  .WithMany(au => au.Articles)
                  .HasForeignKey(a => a.AuthorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed-Daten können hier oder über einen Seeder hinzugefügt werden
    }
}
