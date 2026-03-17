using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace _10_Filmdatenbank.IntegrationTests
{
    public class GroupControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public GroupControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_Returns_ListOfGroups()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.FanGroups.Add(new FanGroup { Name = "Test Group", Description = "A group for testing" });
                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/Group/Discovery");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Test Group");
        }
    }
}
