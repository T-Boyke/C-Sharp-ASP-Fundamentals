using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using _10_Filmdatenbank.Infrastructure.Persistence;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;

namespace _10_Filmdatenbank.IntegrationTests
{
    public class AdminControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AdminControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _client.DefaultRequestHeaders.Add("X-Test-Auto-Auth", "true");
        }

        [Fact]
        public async Task Dashboard_ReturnsSuccess_ForAdmin()
        {
            // Act
            var response = await _client.GetAsync("/Admin/ManageUsers");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("User Management"); // Assuming this is in the view
        }

        [Fact]
        public async Task AddMovie_Post_Redirects_OnSuccess()
        {
            // Act
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "Titel", "New Movie" },
                { "Erscheinungsjahr", "2024" },
                { "Spieldauer", "120" },
                { "Preis", "19.99" }
            });
            var response = await _client.PostAsync("/Movies/Create", content);

            // Assert
            var body = await response.Content.ReadAsStringAsync();
            var matches = System.Text.RegularExpressions.Regex.Matches(body, @"<span[^>]*class=""[^""]*text-danger[^""]*""[^>]*>(.*?)</span>", System.Text.RegularExpressions.RegexOptions.Singleline);
            var errors = string.Join(", ", System.Linq.Enumerable.Where(System.Linq.Enumerable.Select(System.Linq.Enumerable.Cast<System.Text.RegularExpressions.Match>(matches), m => m.Groups[1].Value.Trim()), s => !string.IsNullOrEmpty(s)));
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Redirect, "Validation Errors: " + errors);
            
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Filme.Any(f => f.Titel == "New Movie").Should().BeTrue();
        }
    }
}
