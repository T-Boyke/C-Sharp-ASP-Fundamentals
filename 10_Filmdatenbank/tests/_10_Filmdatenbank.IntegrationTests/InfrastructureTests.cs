using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace _10_Filmdatenbank.IntegrationTests.Infrastructure
{
    public class InfrastructureTests
    {
        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task ApplicationDbContext_Should_Persist_Data()
        {
            // Arrange
            using var context = GetContext();
            var film = new Film { Titel = "Test Film" };

            // Act
            context.Filme.Add(film);
            await context.SaveChangesAsync();

            // Assert
            var persistedFilm = await context.Filme.FirstOrDefaultAsync(f => f.Titel == "Test Film");
            Assert.NotNull(persistedFilm);
            Assert.Equal("Test Film", persistedFilm.Titel);
        }

        [Fact]
        public async Task DbSeeder_Should_Seed_Data()
        {
            // Arrange
            using var context = GetContext();

            // Act
            await DbSeeder.SeedAsync(context);

            // Assert
            Assert.True(await context.Filme.AnyAsync());
            Assert.True(await context.Personen.AnyAsync());
            Assert.True(await context.Eigenschaften.AnyAsync());
            Assert.True(await context.PersonEigenschaftFilme.AnyAsync());
        }

        [Fact]
        public async Task DbSeeder_Should_Not_Seed_Again_If_Data_Exists()
        {
            // Arrange
            using var context = GetContext();
            context.Filme.Add(new Film { Titel = "Existing" });
            await context.SaveChangesAsync();

            // Act
            await DbSeeder.SeedAsync(context);

            // Assert
            Assert.Equal(1, await context.Filme.CountAsync());
        }
    }
}
