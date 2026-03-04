using Xunit;
using _05_NewsApplication.Domain.Entities;

namespace _05_NewsApplication.Tests.Domain {
    public class AuthorTests {
        [Fact]
        public void Fullname_ReturnsFirstnameAndLastnameCombined() {
            // Arrange
            var author = new Author {
                Id = 1,
                Firstname = "John",
                Lastname = "Doe"
            };

            // Act
            var act = author.Fullname;

            // Assert
            Assert.Equal("John Doe", act);
        }

        [Fact]
        public void Fullname_TrimsWhitespace() {
            // Arrange
            var author = new Author {
                Id = 1,
                Firstname = " John ",
                Lastname = " Doe "
            };

            // Act
            var act = author.Fullname;

            // Assert
            Assert.Equal("John   Doe", act); // Depending on implementation Trim() might only trim start/end
        }
    }
}
