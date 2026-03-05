using _06_NewsApplication2.Domain.Entities;

namespace _06_NewsApplication2.Tests.Domain;

public class AuthorTests
{
    [Fact]
    public void Fullname_ShouldReturnFirstAndLastname()
    {
        // Arrange
        var author = new Author 
        { 
            Firstname = "John", 
            Lastname = "Doe" 
        };

        // Act
        var fullname = author.Fullname;

        // Assert
        Assert.Equal("John Doe", fullname);
    }

    [Fact]
    public void Fullname_ShouldHandleEmptyNames()
    {
        // Arrange
        var author = new Author 
        { 
            Firstname = "John", 
            Lastname = "" 
        };

        // Act
        var fullname = author.Fullname;

        // Assert
        Assert.Equal("John", fullname);
    }
}
