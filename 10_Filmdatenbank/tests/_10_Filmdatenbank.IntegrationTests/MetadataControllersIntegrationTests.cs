using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using _10_Filmdatenbank.Infrastructure.Persistence;
using _10_Filmdatenbank.Domain.Entities;
using System.Linq;

namespace _10_Filmdatenbank.IntegrationTests
{
    public class MetadataControllersIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public MetadataControllersIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Collection_Index_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/Kollektionen");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Kollektionen");
        }

        [Fact]
        public async Task ProductionCompany_Index_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/Studios");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Studios");
        }
    }
}
