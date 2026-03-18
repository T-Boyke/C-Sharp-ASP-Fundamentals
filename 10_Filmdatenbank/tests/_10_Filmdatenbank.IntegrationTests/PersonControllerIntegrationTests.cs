using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using _10_Filmdatenbank.Domain.Entities;
using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.IntegrationTests.Controllers
{
    public class PersonControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public PersonControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _client.DefaultRequestHeaders.Add("X-Test-Auto-Auth", "true");
        }

        [Fact]
        public async Task Index_Returns_Success_And_Correct_Content()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Personen.Add(new Person { Vorname = "Christopher", Nachname = "Nolan" });
                await db.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/Schauspieler");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Nolan");
        }

        [Fact]
        public async Task Details_Returns_Success_For_Valid_Id()
        {
            // Arrange
            int personId;
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var person = new Person { Vorname = "Christopher", Nachname = "Nolan" };
                db.Personen.Add(person);
                await db.SaveChangesAsync();
                personId = person.PersonID;
            }

            // Act
            var response = await _client.GetAsync($"/Schauspieler/Details/{personId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Nolan");
        }
    }
}
