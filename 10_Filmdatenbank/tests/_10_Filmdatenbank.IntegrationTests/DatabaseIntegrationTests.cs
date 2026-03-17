using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;

namespace _10_Filmdatenbank.IntegrationTests.Infrastructure
{
    public class DatabaseIntegrationTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public DatabaseIntegrationTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task Can_Add_And_Retrieve_Film_With_Relationships()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                // Arrange
                var genre = new Genre { Name = "Sci-Fi" };
                var film = new Film
                {
                    Titel = "Inception",
                    Erscheinungsjahr = 2010,
                    Preis = 19.99m,
                    Genres = new List<Genre> { genre }
                };

                // Act
                context.Filme.Add(film);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                // Assert
                var retrievedFilm = await context.Filme
                    .Include(f => f.Genres)
                    .FirstOrDefaultAsync(f => f.Titel == "Inception");

                retrievedFilm.Should().NotBeNull();
                retrievedFilm!.Genres.Should().HaveCount(1);
                retrievedFilm.Genres.First().Name.Should().Be("Sci-Fi");
            }
        }

        [Fact]
        public async Task Can_Add_And_Retrieve_Person_With_Awards()
        {
            using (var context = new ApplicationDbContext(_options))
            {
                // Arrange
                var person = new Person
                {
                    Vorname = "Christopher",
                    Nachname = "Nolan",
                    PersonAwards = new List<PersonAward>
                    {
                        new PersonAward { Name = "Best Director", Year = 2011 }
                    }
                };

                // Act
                context.Personen.Add(person);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(_options))
            {
                // Assert
                var retrievedPerson = await context.Personen
                    .Include(p => p.PersonAwards)
                    .FirstOrDefaultAsync(p => p.Nachname == "Nolan");

                retrievedPerson.Should().NotBeNull();
                retrievedPerson!.PersonAwards.Should().HaveCount(1);
                retrievedPerson.PersonAwards.First().Name.Should().Be("Best Director");
            }
        }
    }
}
