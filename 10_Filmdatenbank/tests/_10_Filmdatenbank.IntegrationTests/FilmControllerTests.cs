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

        public FilmControllerTests()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
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
            return new FilmController(
                context, 
                _mockTmdbService.Object, 
                _mockTvdbService.Object, 
                _mockRtService.Object, 
                _mockImdbService.Object, 
                _mockMetacriticService.Object,
                _mockWikidataService.Object, 
                _mockUserManager.Object);
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
            var result = await controller.Edit(film);

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
    }
}
