using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Unit_01_Project_Setup.Tests;

public class ProjectSetupTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProjectSetupTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetVersion_ReturnsSuccessAndCorrectJson()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/version");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<VersionResponse>();
        Assert.NotNull(content);
        Assert.Equal("ASP.NET Core 10", content.Framework);
        Assert.Equal("Healthy", content.Status);
    }

}
