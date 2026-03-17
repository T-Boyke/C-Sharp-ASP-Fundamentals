using _10_Filmdatenbank.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace _10_Filmdatenbank.UnitTests
{
    public class BoundaryValueTests
    {
        [Theory]
        [InlineData(0.00, false)]    // Unter der Grenze (Min 0.01)
        [InlineData(0.01, true)]     // Untere Grenze (Valid)
        [InlineData(500.00, true)]   // Mitte (Valid)
        [InlineData(1000.00, true)]  // Obere Grenze (Valid)
        [InlineData(1000.01, false)] // Über der Grenze (Max 1000.00)
        public void Film_Preis_Boundary_Check(decimal preis, bool expectedValid)
        {
            // Arrange
            var film = new Film 
            { 
                Titel = "Test Film", 
                Preis = preis,
                Spieldauer = 100 // Valid duration
            };
            
            // Act
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(film);
            bool isValid = Validator.TryValidateObject(film, context, validationResults, true);

            // Assert
            Assert.Equal(expectedValid, isValid);
        }

        [Theory]
        [InlineData(0, false)]      // Unter der Grenze (Min 1)
        [InlineData(1, true)]       // Untere Grenze (Valid)
        [InlineData(1000, true)]    // Obere Grenze (Valid)
        [InlineData(1001, false)]   // Über der Grenze (Max 1000)
        public void Film_Spieldauer_Boundary_Check(int dauer, bool expectedValid)
        {
            // Arrange
            var film = new Film 
            { 
                Titel = "Test Film", 
                Preis = 10.00m,
                Spieldauer = dauer 
            };
            
            // Act
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(film);
            bool isValid = Validator.TryValidateObject(film, context, validationResults, true);

            // Assert
            Assert.Equal(expectedValid, isValid);
        }
    }
}
