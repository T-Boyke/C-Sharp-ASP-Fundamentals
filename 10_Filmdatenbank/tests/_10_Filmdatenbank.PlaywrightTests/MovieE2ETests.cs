using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace _10_Filmdatenbank.PlaywrightTests
{
    public class MovieE2ETests : PageTest
    {
        private const string BaseUrl = "http://localhost:5016";

        [Fact]
        public async Task Search_And_Details_Workflow()
        {
            await Page.GotoAsync($"{BaseUrl}/Movies");
            
            var searchInput = Page.GetByPlaceholder("Film suchen...");
            await searchInput.FillAsync("Inception");
            await searchInput.PressAsync("Enter");

            await Page.GetByRole(AriaRole.Link, new() { Name = "Details" }).First.ClickAsync();
            await Expect(Page.Locator(".film-header")).ToBeVisibleAsync();
            await Expect(Page.Locator("h1")).ToContainTextAsync(new System.Text.RegularExpressions.Regex(".+", System.Text.RegularExpressions.RegexOptions.IgnoreCase));
        }

        [Theory]
        [InlineData("Inception", "Inception")]
        [InlineData("The Dark Knight", "The Dark Knight")]
        [InlineData("Matrix", "The Matrix")]
        public async Task Search_Multiple_Movies(string searchQuery, string expectedTitle)
        {
            await Page.GotoAsync($"{BaseUrl}/Movies");
            var searchInput = Page.GetByPlaceholder("Film suchen...");
            await searchInput.FillAsync(searchQuery);
            await searchInput.PressAsync("Enter");

            await Expect(Page.Locator("body")).ToContainTextAsync(expectedTitle);
        }

        [Fact]
        public async Task Ranking_Page_Loads()
        {
            await Page.GotoAsync($"{BaseUrl}/Movies/Ranking");
            await Expect(Page.Locator("h1")).ToContainTextAsync("Top Filme");
            await Expect(Page.Locator(".ranking-list").Or(Page.Locator("table"))).ToBeVisibleAsync();
        }

        [Fact]
        public async Task Admin_Add_And_Delete_Movie()
        {
            // Login as Admin
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("admin@film.de");
            await Page.GetByLabel("Passwort").FillAsync("Admin123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/Movies/Create");
            await Page.GetByLabel("Titel").FillAsync("Playwright Test Movie");
            await Page.GetByLabel("Erscheinungsjahr").FillAsync("2024");
            await Page.GetByLabel("Preis").FillAsync("15,99");
            
            await Page.GetByRole(AriaRole.Button, new() { Name = "Erstellen" }).ClickAsync();

            await Expect(Page).ToHaveURLAsync($"{BaseUrl}/Movies");
            await Expect(Page.Locator("text=Playwright Test Movie")).ToBeVisibleAsync();

            // Delete it
            await Page.Locator("tr").Filter(new() { HasText = "Playwright Test Movie" }).GetByRole(AriaRole.Link, new() { Name = "Löschen" }).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "Löschen" }).ClickAsync();

            await Expect(Page.Locator("text=Playwright Test Movie")).ToHaveCountAsync(0);
        }
    }
}
