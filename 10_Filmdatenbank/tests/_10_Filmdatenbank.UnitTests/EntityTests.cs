using _10_Filmdatenbank.Domain.Entities;
using Xunit;

namespace _10_Filmdatenbank.UnitTests.Domain
{
    public class EntityTests
    {
        [Fact]
        public void Film_Should_Initialize_Correctly()
        {
            // Arrange
            var film = new Film
            {
                FilmID = 1,
                Titel = "Inception",
                Erscheinungsjahr = 2010,
                Spieldauer = 148,
                Preis = 19.99m
            };

            // Assert
            Assert.Equal(1, film.FilmID);
            Assert.Equal("Inception", film.Titel);
            Assert.Equal(2010, film.Erscheinungsjahr);
            Assert.Equal(148, film.Spieldauer);
            Assert.Equal(19.99m, film.Preis);
            Assert.NotNull(film.PersonEigenschaftFilme);
            Assert.Empty(film.PersonEigenschaftFilme);
        }

        [Fact]
        public void Person_Should_Initialize_Correctly()
        {
            // Arrange
            var person = new Person
            {
                PersonID = 1,
                Vorname = "Leonardo",
                Nachname = "DiCaprio"
            };

            // Assert
            Assert.Equal(1, person.PersonID);
            Assert.Equal("Leonardo", person.Vorname);
            Assert.Equal("DiCaprio", person.Nachname);
            Assert.NotNull(person.PersonEigenschaftFilme);
            Assert.Empty(person.PersonEigenschaftFilme);
        }

        [Fact]
        public void Eigenschaft_Should_Initialize_Correctly()
        {
            // Arrange
            var eigenschaft = new Eigenschaft
            {
                EigenschaftID = 1,
                Bezeichnung = "Regisseur"
            };

            // Assert
            Assert.Equal(1, eigenschaft.EigenschaftID);
            Assert.Equal("Regisseur", eigenschaft.Bezeichnung);
            Assert.NotNull(eigenschaft.PersonEigenschaftFilme);
            Assert.Empty(eigenschaft.PersonEigenschaftFilme);
        }

        [Fact]
        public void PersonEigenschaftFilm_Should_Initialize_Correctly()
        {
            // Arrange
            var person = new Person { PersonID = 1 };
            var film = new Film { FilmID = 1 };
            var eigenschaft = new Eigenschaft { EigenschaftID = 1 };
            
            var pef = new PersonEigenschaftFilm
            {
                PEFID = 1,
                PersonID = 1,
                Person = person,
                FilmID = 1,
                Film = film,
                EigenschaftID = 1,
                Eigenschaft = eigenschaft
            };

            // Assert
            Assert.Equal(1, pef.PEFID);
            Assert.Equal(1, pef.PersonID);
            Assert.Equal(person, pef.Person);
            Assert.Equal(1, pef.FilmID);
            Assert.Equal(film, pef.Film);
            Assert.Equal(1, pef.EigenschaftID);
            Assert.Equal(eigenschaft, pef.Eigenschaft);
        }

        [Fact]
        public void ApplicationUser_Should_Initialize_Correctly()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "user1",
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User",
                CreatedAt = DateTime.UtcNow
            };

            // Assert
            Assert.Equal("user1", user.Id);
            Assert.Equal("testuser", user.UserName);
            Assert.Equal("Test", user.FirstName);
            Assert.Equal("User", user.LastName);
            Assert.NotNull(user.FavoriteFilms);
            Assert.NotNull(user.EarnedAchievements);
        }

        [Fact]
        public void Achievement_Should_Initialize_Correctly()
        {
            // Arrange
            var achievement = new Achievement
            {
                AchievementID = 1,
                Name = "Master Collector",
                Description = "Collect 100 films"
            };

            // Assert
            Assert.Equal(1, achievement.AchievementID);
            Assert.Equal("Master Collector", achievement.Name);
            Assert.NotNull(achievement.EarnedBy);
        }

        [Fact]
        public void FavoriteFilm_Should_Initialize_Correctly()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1" };
            var film = new Film { FilmID = 1 };
            var favorite = new FavoriteFilm
            {
                FavoriteFilmID = 1,
                UserID = "user1",
                User = user,
                FilmID = 1,
                Film = film,
                AddedAt = DateTime.UtcNow
            };

            // Assert
            Assert.Equal(1, favorite.FavoriteFilmID);
            Assert.Equal("user1", favorite.UserID);
            Assert.Equal(user, favorite.User);
            Assert.Equal(1, favorite.FilmID);
            Assert.Equal(film, favorite.Film);
        }
    }
}
