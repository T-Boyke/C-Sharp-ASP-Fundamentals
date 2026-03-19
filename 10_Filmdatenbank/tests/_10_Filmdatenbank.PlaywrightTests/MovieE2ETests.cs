using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Threading.Tasks;
using Xunit;

namespace _10_Filmdatenbank.PlaywrightTests
{
    [Collection("SystemTestCollection")]
    /// <summary>
    /// End-to-end tests for movie search, navigation, and administrative workflows.
    /// </summary>
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
            
            // Use a more specific locator to avoid multi-element matches (like language buttons)
            var loginButton = Page.Locator("button[type='submit'].btn-primary");
            await loginButton.ClickAsync();

            try 
            {
                await Expect(Page).Not.ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/Account/Login.*"), new() { Timeout = 10000 });
            }
            catch (System.Exception ex)
            {
                var content = await Page.ContentAsync();
                var url = Page.Url;
                throw new Microsoft.Playwright.PlaywrightException($"Login failed. Still on {url}. Page content snippet: {content.Substring(0, System.Math.Min(1000, content.Length))}. Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Testet den Workflow von der Suche bis zur Detailansicht eines Films.
        /// </summary>
        [Fact]
        public async Task Search_And_Details_Workflow()
        {
            await LoginAsUserAsync();
            await Page.GotoAsync($"{BaseUrl}/Movies");
            
            var searchInput = Page.Locator("input[name='searchString']");
            await searchInput.FillAsync("Inception");
            await searchInput.PressAsync("Enter");
            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/Movies(/Index)?\\?searchString=.*Inception.*", System.Text.RegularExpressions.RegexOptions.IgnoreCase));


            var movieCard = Page.Locator(".card-premium").Filter(new() { HasText = "Inception" }).First;
            await movieCard.HoverAsync();
            var detailsLink = movieCard.Locator("a[href*='/Details/']").Filter(new() { HasText = "Details" }).First;
            await detailsLink.ClickAsync();
            await Page.WaitForURLAsync("**/Details/**");
            await Expect(Page.Locator("h1")).ToContainTextAsync("Inception");
        }

        /// <summary>
        /// Testet die Suche mit verschiedenen Begriffen und validiert das Ergebnis.
        /// </summary>
        /// <param name="searchQuery">Der Suchbegriff.</param>
        /// <param name="expectedTitle">Der erwartete Titel im Ergebnis.</param>
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
            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/Movies(/Index)?\\?searchString=.*", System.Text.RegularExpressions.RegexOptions.IgnoreCase));


            // Assert inside main content to avoid false positives in nav/footer
            await Expect(Page.Locator("main")).ToContainTextAsync(expectedTitle);
        }

        /// <summary>
        /// Überprüft, ob die Ranking-Seite geladen werden kann.
        /// </summary>
        [Fact]
        public async Task Ranking_Page_Loads()
        {
            await LoginAsUserAsync();
            await Page.GotoAsync($"{BaseUrl}/Movies/Ranking");
            await Expect(Page.Locator("h1")).ToContainTextAsync(new System.Text.RegularExpressions.Regex("Top Filme|Ranking", System.Text.RegularExpressions.RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Administrativer Test: Hinzufügen und anschließendes Löschen eines Testfilms.
        /// </summary>
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
            await Page.Locator("#inputTitel").FillAsync(movieTitle);
            await Page.Locator("#inputYear").FillAsync("2024");
            await Page.Locator("#inputPlot").FillAsync("This movie was created by an automated Playwright test.");
            await Page.Locator("#inputRuntime").FillAsync("120");
            await Page.Locator("#inputPrice").FillAsync("15.99");

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

            // Verify success toast appears
            await Expect(Page.Locator(".toast-item")).ToContainTextAsync(movieTitle);

            // Verify movie is gone from the list (check that no card contains this specific unique title)
            await Expect(Page.Locator(".card-premium").Filter(new() { HasText = movieTitle })).ToHaveCountAsync(0);
        }
    }
}
