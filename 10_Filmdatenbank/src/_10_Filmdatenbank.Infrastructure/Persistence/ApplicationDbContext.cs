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
    /// Die Tabelle für Filmkollektionen.
    /// </summary>
    public DbSet<Collection> Collections { get; set; } = null!;

    /// <summary>
    /// Die Tabelle für Produktionsfirmen.
    /// </summary>
    public DbSet<ProductionCompany> ProductionCompanies { get; set; } = null!;

    /// <summary>
    /// Die Tabelle für Filmgenres.
    /// </summary>
    public DbSet<Genre> Genres { get; set; } = null!;

    /// <summary>
    /// Die Tabelle für Filmschlagworte (Keywords).
    /// </summary>
    public DbSet<Keyword> Keywords { get; set; } = null!;

    /// <summary>
    /// Die Tabelle für Länder.
    /// </summary>
    public DbSet<Country> Countries { get; set; } = null!;

    /// <summary>
    /// Die Tabelle für Sprachen.
    /// </summary>
    public DbSet<Language> Languages { get; set; } = null!;

    /// <summary>
    /// Die Tabelle für alternative Filmtitel.
    /// </summary>
    public DbSet<AlternativeTitle> AlternativeTitles { get; set; } = null!;

    /// <summary>
    /// Die Tabelle für länderspezifische Filmveröffentlichungen.
    /// </summary>
    public DbSet<FilmRelease> FilmReleases { get; set; } = null!;

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
                    .HasForeignKey(e => e.FilmID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Eigenschaft)
                    .WithMany(eg => eg.PersonEigenschaftFilme)
                    .HasForeignKey(e => e.EigenschaftID);
            });

        builder.Entity<Film>(entity =>
            {
                entity.HasOne(f => f.Collection)
                    .WithMany(c => c.Films)
                    .HasForeignKey(f => f.CollectionID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(f => f.ProductionCompanies)
                    .WithMany(pc => pc.Films)
                    .UsingEntity(j => j.ToTable("FilmProductionCompanies"));

                entity.HasMany(f => f.Genres)
                    .WithMany(g => g.Films)
                    .UsingEntity(j => j.ToTable("FilmGenres"));

                entity.HasMany(f => f.Keywords)
                    .WithMany(k => k.Films)
                    .UsingEntity(j => j.ToTable("FilmKeywords"));

                entity.HasMany(f => f.ProductionCountries)
                    .WithMany(c => c.ProductionFilms)
                    .UsingEntity(j => j.ToTable("FilmProductionCountries_ISO"));

                entity.HasMany(f => f.SpokenLanguages)
                    .WithMany(l => l.SpokenInFilms)
                    .UsingEntity(j => j.ToTable("FilmSpokenLanguages"));

                entity.HasMany(f => f.SimilarFilms)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("FilmSimilar"));

                entity.HasMany(f => f.RecommendedFilms)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("FilmRecommended"));
            });

        builder.Entity<Genre>()
            .HasIndex(g => g.TmdbId)
            .IsUnique();

        builder.Entity<Keyword>()
            .HasIndex(k => k.TmdbId)
            .IsUnique();

        builder.Entity<Collection>()
            .HasIndex(c => c.TmdbId)
            .IsUnique();

        builder.Entity<ProductionCompany>(entity =>
            {
                entity.HasIndex(pc => pc.TmdbId)
                    .IsUnique();

                entity.HasOne(pc => pc.ParentCompany)
                    .WithMany()
                    .HasForeignKey(pc => pc.ParentCompanyID)
                    .OnDelete(DeleteBehavior.NoAction);
            });

    }
}
