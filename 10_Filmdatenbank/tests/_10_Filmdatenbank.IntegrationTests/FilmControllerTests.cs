using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using _10_Filmdatenbank.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _10_Filmdatenbank.IntegrationTests.Web
{
    public class FilmControllerTests
    {
        private readonly Mock<ITmdbService> _mockTmdbService = new();
        private readonly Mock<ITvdbService> _mockTvdbService = new();
        private readonly Mock<IRottenTomatoesService> _mockRtService = new();
        private readonly Mock<IImdbService> _mockImdbService = new();
        private readonly Mock<IMetacriticService> _mockMetacriticService = new();
        private readonly Mock<IWikidataService> _mockWikidataService = new();
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<FilmController>> _mockLogger = new();

        public FilmControllerTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }

        private ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private FilmController GetController(ApplicationDbContext context)
        {
            var controller = new FilmController(
                context, 
                _mockTmdbService.Object, 
                _mockTvdbService.Object, 
                _mockRtService.Object, 
                _mockImdbService.Object, 
                _mockMetacriticService.Object,
                _mockWikidataService.Object, 
                _mockUserManager.Object,
                _mockLogger.Object);

            controller.TempData = new Mock<ITempDataDictionary>().Object;

            return controller;
        }

        [Fact]
        public async Task Index_Returns_ViewResult_With_Filme()
        {
            // Arrange
            using var context = GetContext();
            context.Filme.Add(new Film { Titel = "B Film" });
            context.Filme.Add(new Film { Titel = "A Film" });
            await context.SaveChangesAsync();
            var controller = GetController(context);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Film>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            Assert.Equal("A Film", model.First().Titel);
        }

        [Fact]
        public async Task Details_Returns_NotFound_If_Id_Invalid()
        {
            // Arrange
            using var context = GetContext();
            var controller = GetController(context);

            // Act
            var result = await controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_Redirects_If_ModelValid()
        {
            // Arrange
            using var context = GetContext();
            var controller = GetController(context);
            var film = new Film { Titel = "New" };

            // Act
            var result = await controller.Create(film);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(1, await context.Filme.CountAsync());
        }

        [Fact]
        public async Task Edit_Post_Redirects_If_ModelValid()
        {
            // Arrange
            using var context = GetContext();
            var film = new Film { Titel = "Old" };
            context.Filme.Add(film);
            await context.SaveChangesAsync();
            
            var controller = GetController(context);
            film.Titel = "Updated";

            // Act
            var result = await controller.Edit(film.FilmID, film);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(1, await context.Filme.CountAsync());
            Assert.Equal("Updated", (await context.Filme.FirstAsync()).Titel);
        }

        [Fact]
        public async Task DeleteConfirmed_Removes_Film()
        {
            // Arrange
            using var context = GetContext();
            var film = new Film { FilmID = 1, Titel = "To Delete" };
            context.Filme.Add(film);
            await context.SaveChangesAsync();
            var controller = GetController(context);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(0, await context.Filme.CountAsync());
        }

        [Fact]
        public async Task Details_Triggers_Enrichment_Paths()
        {
            // Arrange
            using var context = GetContext();
            var film = new Film 
            { 
                FilmID = 1, 
                Titel = "Inception", 
                TmdbId = 27205,
                ImdbId = "tt1375666" 
            };
            context.Filme.Add(film);
            await context.SaveChangesAsync();

            _mockTmdbService.Setup(s => s.GetMovieDetailsAsync(27205))
                .ReturnsAsync(new TMDbLib.Objects.Movies.Movie { Id = 27205, Title = "Inception", VoteAverage = 8.8 });
            
            _mockImdbService.Setup(s => s.GetMetadataAsync("tt1375666"))
                .ReturnsAsync(new _10_Filmdatenbank.Application.Interfaces.ImdbMetadata { Rating = 8.8, Metascore = 74 });

            var controller = GetController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<ViewResult>(result);
            _mockTmdbService.Verify(s => s.GetMovieDetailsAsync(27205), Times.Once);
            _mockImdbService.Verify(s => s.GetMetadataAsync("tt1375666"), Times.Once);
            
            var updatedFilm = await context.Filme.FindAsync(1);
            Assert.NotNull(updatedFilm);
            Assert.Equal(8.8, updatedFilm.ImdbRating);
            Assert.Equal(74, updatedFilm.MetacriticRating);
        }
        [Fact]
        public async Task Index_Search_By_Person_Returns_Correct_Movie_With_Fresh_Context()
        {
            // Arrange
            string dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            using (var seedContext = new ApplicationDbContext(options))
            {
                var nolan = new Person { Vorname = "Christopher", Nachname = "Nolan" };
                seedContext.Personen.Add(nolan);
                var inception = new Film { Titel = "Inception", Erscheinungsjahr = 2010, Preis = 9.99m };
                seedContext.Filme.Add(inception);
                var directorTag = new Eigenschaft { Bezeichnung = "Director" };
                seedContext.Eigenschaften.Add(directorTag);
                await seedContext.SaveChangesAsync();

                seedContext.PersonEigenschaftFilme.Add(new PersonEigenschaftFilm
                {
                    Film = inception,
                    Person = nolan,
                    Eigenschaft = directorTag
                });
                await seedContext.SaveChangesAsync();
            }

            // Act - USE FRESH CONTEXT
            using (var context = new ApplicationDbContext(options))
            {
                var controller = GetController(context);
                var result = await controller.Index("Christopher Nolan");

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Film>>(viewResult.ViewData.Model);
                Assert.Single(model);
                Assert.Equal("Inception", model.First().Titel);
            }
        }
        [Fact]
        public async Task Index_Search_After_DbSeeder_Returns_Correct_Movie()
        {
            // Arrange
            string dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await DbSeeder.SeedAsync(context);
            }

            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var controller = GetController(context);
                var result = await controller.Index("Christopher Nolan");

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Film>>(viewResult.ViewData.Model);
                Assert.Contains(model, f => f.Titel == "Inception");
            }
        }
    }
}
