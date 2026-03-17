using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using _10_Filmdatenbank.Infrastructure.Persistence;
using System.Linq;

namespace _10_Filmdatenbank.IntegrationTests
{
    public class AdminControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AdminControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Dashboard_ReturnsSuccess_ForAdmin()
        {
            // Act
            var response = await _client.GetAsync("/Admin/Index");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Dashboard");
        }

        [Fact]
        public async Task AddMovie_Post_Redirects_OnSuccess()
        {
            // Act
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "Titel", "New Movie" },
                { "Erscheinungsdatum", "2024-01-01" }
            });
            var response = await _client.PostAsync("/Admin/AddMovie", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Filme.Any(f => f.Titel == "New Movie").Should().BeTrue();
        }
    }
}
