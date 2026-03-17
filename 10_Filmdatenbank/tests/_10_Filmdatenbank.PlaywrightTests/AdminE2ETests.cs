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

        [Fact]
        public async Task Admin_UserManagement_Workflow()
        {
            // Login as Admin
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("admin@film.de");
            await Page.GetByLabel("Passwort").FillAsync("Admin123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/Admin/ManageUsers");
            await Expect(Page.Locator("h1")).ToContainTextAsync("Benutzerverwaltung");

            // Toggle status for the regular user
            var userRow = Page.Locator("tr").Filter(new() { HasText = "user@film.de" });
            var toggleButton = userRow.GetByRole(AriaRole.Button).Filter(new() { HasText = "Deaktivieren" }).Or(userRow.GetByRole(AriaRole.Button).Filter(new() { HasText = "Aktivieren" }));
            
            await toggleButton.ClickAsync();
            await Expect(Page).ToHaveURLAsync($"{BaseUrl}/Admin/ManageUsers");
        }

        [Fact]
        public async Task Admin_Maintenance_Settings()
        {
            // Login as Admin
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("admin@film.de");
            await Page.GetByLabel("Passwort").FillAsync("Admin123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/Admin/Settings");
            await Expect(Page.Locator("h1")).ToContainTextAsync("Systemeinstellungen");

            // Test clearing films (will show success message)
            await Page.GetByRole(AriaRole.Button, new() { Name = "Alle Filme löschen" }).ClickAsync();
            await Expect(Page.Locator("text=Alle Filme wurden unwiderruflich gelöscht.")).ToBeVisibleAsync();
        }
    }
}
