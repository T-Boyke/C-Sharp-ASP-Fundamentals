using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace _10_Filmdatenbank.PlaywrightTests
{
    [Collection("SystemTestCollection")]
    public class MovieE2ETests : PageTest
    {
        private readonly string BaseUrl;

        public MovieE2ETests(Infrastructure.TestHost<Program> host)
        {
            BaseUrl = host.BaseUrl;
        }

        private async Task LoginAsUserAsync()
        {
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.Locator("input[type='email']").FillAsync("user@film.de");
            await Page.Locator("input[type='password']").FillAsync("User123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).Or(Page.Locator("button[type='submit']")).First.ClickAsync();
            await Expect(Page).Not.ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/Account/Login.*"));
        }

        [Fact]
        public async Task Search_And_Details_Workflow()
        {
            await LoginAsUserAsync();
            await Page.GotoAsync($"{BaseUrl}/Movies");
            
            var searchInput = Page.Locator("input[name='searchString']");
            await searchInput.FillAsync("Inception");
            await searchInput.PressAsync("Enter");

            var detailsLink = Page.GetByLabel(new System.Text.RegularExpressions.Regex("DetailsOf.*|Details von.*", System.Text.RegularExpressions.RegexOptions.IgnoreCase)).First;
            await detailsLink.ClickAsync(new() { Force = true });
            await Page.WaitForURLAsync("**/Details/**");
            await Expect(Page.Locator("h1")).ToContainTextAsync("Inception");
        }

        [Theory]
        [InlineData("Inception", "Inception")]
        [InlineData("Christopher Nolan", "Inception")]
        public async Task Search_Multiple_Movies(string searchQuery, string expectedTitle)
        {
            await LoginAsUserAsync();
            await Page.GotoAsync($"{BaseUrl}/Movies");
            
            var searchInput = Page.Locator("input[name='searchString']");
            await searchInput.FillAsync(searchQuery);
            await searchInput.PressAsync("Enter");

            await Expect(Page.Locator("body")).ToContainTextAsync(expectedTitle);
        }

        [Fact]
        public async Task Ranking_Page_Loads()
        {
            await LoginAsUserAsync();
            await Page.GotoAsync($"{BaseUrl}/Movies/Ranking");
            await Expect(Page.Locator("h1")).ToContainTextAsync(new System.Text.RegularExpressions.Regex("Top Filme|Ranking", System.Text.RegularExpressions.RegexOptions.IgnoreCase));
        }

        [Fact]
        public async Task Admin_Add_And_Delete_Movie()
        {
            // Login as Admin
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.Locator("input[type='email']").FillAsync("admin@film.de");
            await Page.Locator("input[type='password']").FillAsync("Admin123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).Or(Page.Locator("button[type='submit']")).First.ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/Movies/Create");
            await Page.Locator("input[name='Titel']").FillAsync("Playwright Test Movie");
            await Page.Locator("input[name='Erscheinungsjahr']").FillAsync("2024");
            await Page.Locator("input[name='Preis']").FillAsync("15.99");
            
            await Page.GetByRole(AriaRole.Button, new() { Name = "Erstellen" }).Or(Page.Locator("button[type='submit']")).First.ClickAsync(new() { Force = true });

            try
            {
                await Expect(Page).ToHaveURLAsync($"{BaseUrl}/Movies");
            }
            catch
            {
                var errors = await Page.Locator(".text-danger, .validation-summary-errors").AllInnerTextsAsync();
                var errorMsg = string.Join(" | ", errors);
                throw new Microsoft.Playwright.PlaywrightException($"Expected URL /Movies but got {Page.Url}. Validation Errors: {errorMsg}");
            }
            await Expect(Page.Locator("body")).ToContainTextAsync("Playwright Test Movie");

            // Delete it
            await Page.Locator(".card-premium, tr").Filter(new() { HasText = "Playwright Test Movie" }).GetByRole(AriaRole.Link, new() { Name = "Löschen" }).Or(Page.Locator("a:text('Löschen')")).First.ClickAsync(new() { Force = true });
            await Page.GetByRole(AriaRole.Button, new() { Name = "Löschen" }).Or(Page.Locator("button[type='submit']")).First.ClickAsync(new() { Force = true });

            await Expect(Page.Locator("body")).Not.ToContainTextAsync("Playwright Test Movie");
        }
    }
}
