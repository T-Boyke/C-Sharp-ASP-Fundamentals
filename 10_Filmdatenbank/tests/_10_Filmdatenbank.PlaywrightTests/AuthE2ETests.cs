using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace _10_Filmdatenbank.PlaywrightTests
{
    [Collection("SystemTestCollection")]
    public class AuthE2ETests : PageTest
    {
        private readonly string BaseUrl;

        public AuthE2ETests(Infrastructure.TestHost<Program> host)
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
        public async Task Login_Logout_Workflow()
        {
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("admin@film.de");
            await Page.GetByLabel("Passwort", new() { Exact = true }).FillAsync("Admin123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Expect(Page).ToHaveURLAsync($"{BaseUrl}/User/Dashboard");
            await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "System Administrator", Exact = true })).ToBeVisibleAsync();

            // Logout - open dropdown first
            await Page.GetByRole(AriaRole.Button, new() { Name = "System" }).ClickAsync();
            var logoutButton = Page.Locator("button[title='Abmelden']").Or(Page.Locator("form[action='/Account/Logout'] button"));
            await logoutButton.ClickAsync();
            await Expect(Page).ToHaveURLAsync(BaseUrl);
        }

        [Fact]
        public async Task Update_User_Profile()
        {
            // Login first
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("user@film.de");
            await Page.GetByLabel("Passwort", new() { Exact = true }).FillAsync("User123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/User/Profile");
            await Page.GetByRole(AriaRole.Link, new() { Name = "Profil bearbeiten" }).Or(Page.Locator("text=Profil bearbeiten")).ClickAsync();

            await Page.GetByLabel("Vorname").FillAsync("UpdatedName");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Speichern" }).ClickAsync();

            // Use a more specific locator to avoid multi-match errors
            await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "UpdatedName User", Exact = true })).ToBeVisibleAsync();
        }

        [Fact]
        public async Task Change_Language_And_Theme_Settings()
        {
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("user@film.de");
            await Page.GetByLabel("Passwort", new() { Exact = true }).FillAsync("User123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/User/Settings");
            
            // Select English - use ID and First() to handle responsive duplicates
            await Page.Locator("#PreferredLanguage").First.SelectOptionAsync(new[] { "en" });
            
            // Selection for Theme (Radio) - use Force because of sr-only and sticky nav
            await Page.Locator("input[value='coral']").First.CheckAsync(new() { Force = true });
            
            await Page.GetByRole(AriaRole.Button, new() { Name = "Änderungen speichern" })
                .Or(Page.GetByRole(AriaRole.Button, new() { Name = "Speichern" }))
                .Or(Page.GetByRole(AriaRole.Button, new() { Name = "Save Changes" }))
                .First.ClickAsync(new() { Force = true });

            var successMsg = Page.Locator("text=Einstellungen wurden gespeichert.").Or(Page.Locator("text=Settings saved."));
            await Expect(successMsg.First).ToBeVisibleAsync();
        }
    }
}
