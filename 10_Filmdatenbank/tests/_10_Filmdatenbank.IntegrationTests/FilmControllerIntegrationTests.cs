using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using _10_Filmdatenbank.Domain.Entities;
using System;

namespace _10_Filmdatenbank.IntegrationTests
{
    public class FilmControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public FilmControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Index_Returns_Success_And_Correct_Content()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Filme.Add(new Film { Titel = "Inception", Erscheinungsjahr = 2010, Preis = 19.99m });
                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/Movies");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Inception");
        }

        [Fact]
        public async Task Details_Returns_NotFound_For_Invalid_Id()
        {
            // Act
            var response = await _client.GetAsync("/Movies/Details/999");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Ranking_Returns_Success()
        {
            // Act
            var response = await _client.GetAsync("/Movies/Ranking");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
