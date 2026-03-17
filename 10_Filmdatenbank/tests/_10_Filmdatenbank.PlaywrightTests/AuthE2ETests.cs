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

        [Fact]
        public async Task Login_Logout_Workflow()
        {
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.Locator("input[type='email']").FillAsync("admin@film.de");
            await Page.Locator("input[type='password']").FillAsync("Admin123!");
            await Page.Locator("button[type='submit']").First.ClickAsync();

            await Expect(Page).ToHaveURLAsync(BaseUrl);
            await Expect(Page.Locator("text=System Administrator")).ToBeVisibleAsync();

            // Logout
            var logoutButton = Page.GetByRole(AriaRole.Button, new() { Name = "Abmelden" }).Or(Page.Locator("form[action='/Account/Logout'] button"));
            await logoutButton.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/Account/Login"));
        }

        [Fact]
        public async Task Update_User_Profile()
        {
            // Login first
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("user@film.de");
            await Page.GetByLabel("Passwort").FillAsync("User123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/User/Profile");
            await Page.GetByRole(AriaRole.Link, new() { Name = "Profil bearbeiten" }).Or(Page.Locator("text=Profil bearbeiten")).ClickAsync();

            await Page.GetByLabel("Vorname").FillAsync("UpdatedName");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Speichern" }).ClickAsync();

            await Expect(Page.Locator("text=UpdatedName")).ToBeVisibleAsync();
        }

        [Fact]
        public async Task Change_Language_And_Theme_Settings()
        {
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("user@film.de");
            await Page.GetByLabel("Passwort").FillAsync("User123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/User/Settings");
            
            // Select English
            await Page.GetByLabel("Bevorzugte Sprache").Or(Page.Locator("#PreferredLanguage")).SelectOptionAsync(new[] { "en" });
            // Select Dark Theme
            await Page.GetByLabel("Theme").Or(Page.Locator("#Theme")).SelectOptionAsync(new[] { "dark" });
            
            await Page.GetByRole(AriaRole.Button, new() { Name = "Speichern" }).ClickAsync();

            var successMsg = Page.Locator("text=Einstellungen wurden gespeichert.").Or(Page.Locator("text=Settings saved."));
            await Expect(successMsg).ToBeVisibleAsync();
        }
    }
}
