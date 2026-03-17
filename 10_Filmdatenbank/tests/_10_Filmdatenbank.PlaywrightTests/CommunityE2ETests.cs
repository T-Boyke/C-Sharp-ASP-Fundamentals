using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace _10_Filmdatenbank.PlaywrightTests
{
    public class CommunityE2ETests : PageTest
    {
        private const string BaseUrl = "http://localhost:5016";

        [Fact]
        public async Task Community_Group_Interactions()
        {
            // Login
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.GetByLabel("Email").FillAsync("user@film.de");
            await Page.GetByLabel("Passwort").FillAsync("User123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).ClickAsync();

            await Page.GotoAsync($"{BaseUrl}/Group/Discovery");
            
            // Create a group
            await Page.GetByRole(AriaRole.Link, new() { Name = "Neue Gruppe gründen" }).ClickAsync();
            await Page.GetByLabel("Name der Gruppe").FillAsync("Test Fan Group");
            await Page.GetByLabel("Beschreibung").FillAsync("A group for E2E testing.");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Gründen" }).ClickAsync();

            await Expect(Page.Locator("h1")).ToContainTextAsync("Test Fan Group");

            // Post a thread
            await Page.GetByRole(AriaRole.Link, new() { Name = "Neue Diskussion" }).ClickAsync();
            await Page.GetByLabel("Titel").FillAsync("E2E Discussion");
            await Page.GetByLabel("Inhalt").FillAsync("Something interesting here.");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Erstellen" }).ClickAsync();

            await Expect(Page.Locator("h1")).ToContainTextAsync("E2E Discussion");

            // Post a comment
            await Page.GetByPlaceholder("Schreibe einen Kommentar...").FillAsync("My E2E comment");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Posten" }).ClickAsync();

            await Expect(Page.Locator("text=My E2E comment")).ToBeVisibleAsync();
        }
    }
}
