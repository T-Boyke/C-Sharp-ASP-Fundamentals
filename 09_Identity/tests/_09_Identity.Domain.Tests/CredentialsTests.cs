using _09_Identity.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace _09_Identity.Domain.Tests;

public class CredentialsTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateInstance()
    {
        // Act
        var credentials = new Credentials("Admin", "Pass123!");

        // Assert
        credentials.Username.Should().Be("Admin");
        credentials.Password.Should().Be("Pass123!");
    }

    [Theory]
    [InlineData("", "pass")]
    [InlineData("user", "")]
    [InlineData(null, "pass")]
    public void Constructor_WithInvalidData_ShouldThrowArgumentException(string? user, string? pass)
    {
        // Act
        Action act = () => new Credentials(user!, pass!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
