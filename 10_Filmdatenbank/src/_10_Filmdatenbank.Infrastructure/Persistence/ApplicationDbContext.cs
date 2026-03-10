using _10_Filmdatenbank.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _10_Filmdatenbank.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Film> Filme { get; set; } = null!;
        public DbSet<Person> Personen { get; set; } = null!;
        public DbSet<Eigenschaft> Eigenschaften { get; set; } = null!;
        public DbSet<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
}
