using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace _10_Filmdatenbank.IntegrationTests.Controllers
{
    public class HomeControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public HomeControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Index_Returns_Success()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Privacy_Returns_Success()
        {
            var response = await _client.GetAsync("/Home/Privacy");
            response.EnsureSuccessStatusCode();
        }
    }
}
