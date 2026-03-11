using _10_Filmdatenbank.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _10_Filmdatenbank.Infrastructure.Persistence;

/// <summary>
/// Der Entity Framework Core Datenbankkontext für die Anwendung.
/// Erweitert IdentityDbContext für die Benutzer- und Rollenverwaltung.
/// </summary>
public class ApplicationDbContext : IdentityDbContext
{
    /// <summary>
    /// Initialisiert eine neue Instanz des Datenbankkontexts.
    /// </summary>
    /// <param name="options">Die Optionen für diesen Kontext.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Die Tabelle für Filme.
    /// </summary>
    public DbSet<Film> Filme { get; set; } = null!;

    /// <summary>
    /// Die Tabelle für Personen.
    /// </summary>
    public DbSet<Person> Personen { get; set; } = null!;

    /// <summary>
    /// Die Tabelle für Eigenschaften (Rollen).
    /// </summary>
    public DbSet<Eigenschaft> Eigenschaften { get; set; } = null!;

    /// <summary>
    /// Die Verknüpfungstabelle für Personen, Filme und deren Eigenschaften.
    /// </summary>
    public DbSet<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = null!;

    /// <summary>
    /// Konfiguriert das Datenbankmodell, insbesondere Beziehungen und initiale Daten.
    /// </summary>
    /// <param name="builder">Der ModelBuilder zum Konfigurieren des Modells.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Film>()
            .Property(f => f.Preis)
            .HasPrecision(18, 2);

        builder.Entity<PersonEigenschaftFilm>(entity =>
            {
                entity.HasKey(e => e.PEFID);

                entity.HasOne(e => e.Person)
                    .WithMany(p => p.PersonEigenschaftFilme)
                    .HasForeignKey(e => e.PersonID);

                entity.HasOne(e => e.Film)
                    .WithMany(f => f.PersonEigenschaftFilme)
                    .HasForeignKey(e => e.FilmID);

                entity.HasOne(e => e.Eigenschaft)
                    .WithMany(eg => eg.PersonEigenschaftFilme)
                    .HasForeignKey(e => e.EigenschaftID);
            });

        // Seed initial properties/roles as per assignment
        builder.Entity<Eigenschaft>().HasData(
            new Eigenschaft { EigenschaftID = 1, Bezeichnung = "Regisseur" },
            new Eigenschaft { EigenschaftID = 2, Bezeichnung = "Produzent" },
            new Eigenschaft { EigenschaftID = 3, Bezeichnung = "Schauspieler" }
        );
    }
}
