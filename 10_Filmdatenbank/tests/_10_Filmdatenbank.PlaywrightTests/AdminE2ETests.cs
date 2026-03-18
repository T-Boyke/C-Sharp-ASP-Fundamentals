using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace _10_Filmdatenbank.PlaywrightTests
{
    [Collection("SystemTestCollection")]
    public class AdminE2ETests : PageTest
    {
        private readonly string BaseUrl;

        public AdminE2ETests(Infrastructure.TestHost<Program> host)
        {
            BaseUrl = host.BaseUrl;
        }

        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions
            {
                Locale = "de-DE"
            };
        }

        [Fact]
        public async Task Admin_UserManagement_Workflow()
        {
            // Login as Admin
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("admin@film.de");
            await Page.GetByLabel("Passwort", new() { Exact = true }).FillAsync("Admin123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/Admin/ManageUsers");
            await Expect(Page.Locator("h1")).ToContainTextAsync("User Management");

            // Toggle status for the regular user using the title attribute
            var userRow = Page.Locator("tr").Filter(new() { HasText = "user@film.de" });
            var toggleButton = userRow.Locator("button[title='Deaktivieren']").Or(userRow.Locator("button[title='Aktivieren']"));
            
            await toggleButton.ClickAsync();
            await Expect(Page).ToHaveURLAsync($"{BaseUrl}/Admin/ManageUsers");
        }

        [Fact]
        public async Task Admin_Maintenance_Settings()
        {
            // Login as Admin
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("admin@film.de");
            await Page.GetByLabel("Passwort", new() { Exact = true }).FillAsync("Admin123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/Admin/Settings");
            await Expect(Page.Locator("h1")).ToContainTextAsync("System Settings");

            // Test clearing films: find the button by its onclick action
            await Page.Locator("button[onclick*='ClearFilms']").ClickAsync();

            // Modal should appear
            await Expect(Page.Locator("#actionConfirmModal")).ToBeVisibleAsync();
            await Page.Locator("#actionConfirmInput").FillAsync("DELETE-FILMS");
            await Page.Locator("#finalActionBtn").ClickAsync();

            // Redirects with success message
            await Expect(Page.Locator("text=Alle Filme wurden unwiderruflich gelöscht.")).ToBeVisibleAsync();
        }
    }
}
