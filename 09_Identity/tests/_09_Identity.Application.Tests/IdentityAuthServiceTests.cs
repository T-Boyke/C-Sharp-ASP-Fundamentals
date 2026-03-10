using _09_Identity.Domain.Interfaces;
using _09_Identity.Domain.ValueObjects;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace _09_Identity.Application.Tests;

public class IdentityAuthServiceTests
{
    private readonly IAuthService _authServiceMock;

    public IdentityAuthServiceTests()
    {
        _authServiceMock = Substitute.For<IAuthService>();
    }

    [Fact]
    public async Task LoginAsync_WithMock_ShouldReturnExpectedResult()
    {
        // Arrange
        var credentials = new Credentials("Admin", "Pass123!");
        _authServiceMock.LoginAsync(credentials).Returns(true);

        // Act
        var result = await _authServiceMock.LoginAsync(credentials);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task LogoutAsync_ShouldExecute()
    {
        // Act
        await _authServiceMock.LogoutAsync();

        // Assert
        await _authServiceMock.Received(1).LogoutAsync();
    }
}
