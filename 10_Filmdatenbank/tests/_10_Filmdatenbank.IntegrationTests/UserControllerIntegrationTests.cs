using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using _10_Filmdatenbank.Domain.Entities;
using System;
using System.Linq;

namespace _10_Filmdatenbank.IntegrationTests.Controllers
{
    public class UserControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public UserControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Dashboard_Returns_Success_For_Authenticated_User()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!db.Users.Any(u => u.Id == "test-user-id"))
                {
                    db.Users.Add(new ApplicationUser 
                    { 
                        Id = "test-user-id", 
                        UserName = "test@user.com", 
                        Email = "test@user.com" 
                    });
                    await db.SaveChangesAsync();
                }
            }

            // Act
            var response = await _client.GetAsync("/User/Dashboard");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Profile_Returns_Success()
        {
            // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!db.Users.Any(u => u.Id == "test-user-id"))
                {
                    db.Users.Add(new ApplicationUser 
                    { 
                        Id = "test-user-id", 
                        UserName = "test@user.com", 
                        Email = "test@user.com" 
                    });
                    await db.SaveChangesAsync();
                }
            }

            // Act
            var response = await _client.GetAsync("/User/Profile");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Settings_Returns_Success()
        {
             // Arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!db.Users.Any(u => u.Id == "test-user-id"))
                {
                    db.Users.Add(new ApplicationUser 
                    { 
                        Id = "test-user-id", 
                        UserName = "test@user.com", 
                        Email = "test@user.com" 
                    });
                    await db.SaveChangesAsync();
                }
            }

            // Act
            var response = await _client.GetAsync("/User/Settings");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
