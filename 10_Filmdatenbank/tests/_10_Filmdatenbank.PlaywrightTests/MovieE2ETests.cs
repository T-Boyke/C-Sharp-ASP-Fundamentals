using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Threading.Tasks;
using Xunit;

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
            await Expect(Page).Not.ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/Account/Login.*"));

            // Admin: Film erstellen und löschen
            // Create a unique title for this test run to avoid interference
            string uniqueId = System.Guid.NewGuid().ToString().Substring(0, 8);
            string movieTitle = $"Playwright Test Movie {uniqueId}";

            await Page.GotoAsync($"{BaseUrl}/Movies/Create");
            await Page.Locator("#Titel").FillAsync(movieTitle);
            await Page.Locator("#Erscheinungsjahr").FillAsync("2024");
            await Page.Locator("#Handlung").FillAsync("This movie was created by an automated Playwright test.");
            await Page.Locator("#Spieldauer").FillAsync("120");
            await Page.Locator("#Preis").FillAsync("15.99");

            // Click Create
            await Page.Locator("#createForm button[type='submit']").ClickAsync();

            // Wait for redirect to /Movies or /Movies/Index
            try
            {
                await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex($".*/Movies(/Index)?$"));
            }
            catch (System.Exception ex)
            {
                // Better error reporting for validation errors
                var validationSummary = await Page.Locator(".text-danger-primary, .validation-summary-errors, .field-validation-error").AllInnerTextsAsync();
                var pageContent = await Page.Locator("body").InnerTextAsync();
                var currentUrl = Page.Url;
                throw new Microsoft.Playwright.PlaywrightException(
                    $"Expected URL /Movies but got {currentUrl}. Validation Errors: {string.Join(", ", validationSummary)}. Exception: {ex.Message}. Page Body Snippet: {pageContent.Substring(0, System.Math.Min(500, pageContent.Length))}");
            }

            // Verify movie exists in list
            await Expect(Page.Locator("body")).ToContainTextAsync(movieTitle);

            // Delete it
            await Page.Locator(".card-premium, tr").Filter(new() { HasText = movieTitle }).Locator("a[href*='Delete']").First.ClickAsync(new() { Force = true });
            
            // Confirm Delete
            await Page.Locator("button[type='submit']").Filter(new() { HasText = "Löschen" }).Or(Page.Locator("form[action*='Delete'] button[type='submit']")).First.ClickAsync(new() { Force = true });

            // Ensure we are back on the movies list
            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex($".*/Movies(/Index)?$"));

            // Verify movie is gone from the list
            await Expect(Page.Locator("body")).Not.ToContainTextAsync(movieTitle);
        }
    }
}
