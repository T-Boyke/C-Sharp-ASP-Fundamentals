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
            _client.DefaultRequestHeaders.Add("X-Test-Auto-Auth", "true");
        }

        [Fact]
        public async Task Dashboard_Returns_Success_For_Authenticated_User()
        {
            // Act
            var response = await _client.GetAsync("/User/Dashboard");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Profile_Returns_Success()
        {
            // Act
            var response = await _client.GetAsync("/User/Profile");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Settings_Returns_Success()
        {
            // Act
            var response = await _client.GetAsync("/User/Settings");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
