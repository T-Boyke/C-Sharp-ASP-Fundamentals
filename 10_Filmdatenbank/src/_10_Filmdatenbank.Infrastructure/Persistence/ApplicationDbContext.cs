using _10_Filmdatenbank.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _10_Filmdatenbank.Infrastructure.Persistence;

/// <summary>
/// Der Entity Framework Core Datenbankkontext für die Anwendung.
/// Erweitert IdentityDbContext für die Benutzer- und Rollenverwaltung.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Initialisiert eine neue Instanz des Datenbankkontexts.
    /// </summary>
    /// <param name="options">Die Optionen für diesen Kontext.</param>
    public ApplicationDbContext(DbContextOptions options)
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

    // Community / Social DBSets
    public DbSet<FanGroup> FanGroups { get; set; } = null!;
    public DbSet<GroupMember> GroupMembers { get; set; } = null!;
    public DbSet<DiscussionThread> DiscussionThreads { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<MembershipRequest> MembershipRequests { get; set; } = null!;
    public DbSet<GroupBan> GroupBans { get; set; } = null!;
    public DbSet<Achievement> Achievements { get; set; } = null!;
    public DbSet<UserAchievement> UserAchievements { get; set; } = null!;
    public DbSet<FavoriteFilm> FavoriteFilms { get; set; } = null!;

    /// <summary>
    /// Konfiguriert das Datenbankmodell, insbesondere Beziehungen und initiale Daten.
    /// </summary>
    /// <param name="builder">Der ModelBuilder zum Konfigurieren des Modells.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Film>(entity =>
        {
            entity.HasKey(f => f.FilmID);
            entity.Property(f => f.Preis).HasPrecision(18, 2);
            
            entity.HasOne(f => f.Collection)
                .WithMany(c => c.Films)
                .HasForeignKey(f => f.CollectionID)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(f => f.ProductionCompanies)
                .WithMany(pc => pc.Films)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmProductionCompanies",
                    j => j.HasOne<ProductionCompany>().WithMany().HasForeignKey("ProductionCompanyID"),
                    j => j.HasOne<Film>().WithMany().HasForeignKey("FilmID"),
                    j => j.ToTable("FilmProductionCompanies"));

            entity.HasMany(f => f.Genres)
                .WithMany(g => g.Films)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmGenres",
                    j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreID"),
                    j => j.HasOne<Film>().WithMany().HasForeignKey("FilmID"),
                    j => j.ToTable("FilmGenres"));

            entity.HasMany(f => f.Keywords)
                .WithMany(k => k.Films)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmKeywords",
                    j => j.HasOne<Keyword>().WithMany().HasForeignKey("KeywordID"),
                    j => j.HasOne<Film>().WithMany().HasForeignKey("FilmID"),
                    j => j.ToTable("FilmKeywords"));

            entity.HasMany(f => f.ProductionCountries)
                .WithMany(c => c.ProductionFilms)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmProductionCountries_ISO",
                    j => j.HasOne<Country>().WithMany().HasForeignKey("Iso_3166_1"),
                    j => j.HasOne<Film>().WithMany().HasForeignKey("FilmID"),
                    j => j.ToTable("FilmProductionCountries_ISO"));

            entity.HasMany(f => f.SpokenLanguages)
                .WithMany(l => l.SpokenInFilms)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmSpokenLanguages",
                    j => j.HasOne<Language>().WithMany().HasForeignKey("Iso_639_1"),
                    j => j.HasOne<Film>().WithMany().HasForeignKey("FilmID"),
                    j => j.ToTable("FilmSpokenLanguages"));

            entity.HasMany(f => f.SimilarFilms)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "FilmSimilar",
                    j => j.HasOne<Film>().WithMany().HasForeignKey("SimilarFilmID").OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<Film>().WithMany().HasForeignKey("FilmID").OnDelete(DeleteBehavior.NoAction),
                    j => j.ToTable("FilmSimilar"));

            entity.HasMany(f => f.RecommendedFilms)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "FilmRecommended",
                    j => j.HasOne<Film>().WithMany().HasForeignKey("RecommendedFilmID").OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<Film>().WithMany().HasForeignKey("FilmID").OnDelete(DeleteBehavior.NoAction),
                    j => j.ToTable("FilmRecommended"));
        });

        builder.Entity<Person>(entity => entity.HasKey(p => p.PersonID));
        builder.Entity<Genre>(entity => entity.HasKey(g => g.GenreID));
        builder.Entity<Keyword>(entity => entity.HasKey(k => k.KeywordID));
        builder.Entity<Collection>(entity => entity.HasKey(c => c.CollectionID));
        builder.Entity<ProductionCompany>(entity => entity.HasKey(pc => pc.ProductionCompanyID));
        builder.Entity<Eigenschaft>(entity => entity.HasKey(e => e.EigenschaftID));
        builder.Entity<Country>(entity => entity.HasKey(c => c.Iso_3166_1));
        builder.Entity<Language>(entity => entity.HasKey(l => l.Iso_639_1));
        builder.Entity<AlternativeTitle>(entity => entity.HasKey(at => at.AlternativeTitleID));
        builder.Entity<FilmRelease>(entity => entity.HasKey(r => r.ReleaseID));

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

        // Community Configurations
        builder.Entity<FanGroup>(entity =>
        {
            entity.HasKey(g => g.FanGroupID);
            entity.HasOne(g => g.ParentGroup)
                .WithMany(g => g.SubGroups)
                .HasForeignKey(g => g.ParentGroupID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<DiscussionThread>(entity => 
        {
            entity.HasKey(t => t.ThreadID);
            entity.HasOne(t => t.Author)
                .WithMany(u => u.Threads)
                .HasForeignKey(t => t.AuthorID)
                .OnDelete(DeleteBehavior.NoAction);
                
            entity.HasOne(t => t.FanGroup)
                .WithMany(g => g.Threads)
                .HasForeignKey(t => t.FanGroupID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Comment>(entity => 
        {
            entity.HasKey(c => c.CommentID);
            entity.HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(c => c.Thread)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.ThreadID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Notification>(entity => 
        {
            entity.HasKey(n => n.NotificationID);
            entity.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<GroupMember>(entity => entity.HasKey(m => m.GroupMemberID));

        builder.Entity<GroupMember>(entity =>
        {
            entity.HasOne(m => m.User)
                .WithMany(u => u.GroupMemberships)
                .HasForeignKey(m => m.UserID);

            entity.HasOne(m => m.FanGroup)
                .WithMany(g => g.Members)
                .HasForeignKey(m => m.FanGroupID);
        });

        builder.Entity<MembershipRequest>(entity => 
        {
            entity.HasKey(r => r.MembershipRequestID);
            entity.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(r => r.FanGroup)
                .WithMany(g => g.JoinRequests)
                .HasForeignKey(r => r.FanGroupID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<GroupBan>(entity => 
        {
            entity.HasKey(b => b.GroupBanID);
            entity.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(b => b.FanGroup)
                .WithMany(g => g.BannedUsers)
                .HasForeignKey(b => b.FanGroupID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Achievement>(entity => entity.HasKey(a => a.AchievementID));
        
        builder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(ua => ua.UserAchievementID);
            entity.HasOne(ua => ua.User)
                .WithMany(u => u.EarnedAchievements)
                .HasForeignKey(ua => ua.UserID)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(ua => ua.Achievement)
                .WithMany(a => a.EarnedBy)
                .HasForeignKey(ua => ua.AchievementID)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<FavoriteFilm>(entity =>
        {
            entity.HasKey(ff => ff.FavoriteFilmID);
            entity.HasOne(ff => ff.User)
                .WithMany(u => u.FavoriteFilms)
                .HasForeignKey(ff => ff.UserID)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(ff => ff.Film)
                .WithMany()
                .HasForeignKey(ff => ff.FilmID)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
