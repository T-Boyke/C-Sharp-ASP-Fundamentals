using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace _10_Filmdatenbank.PlaywrightTests
{
    [Collection("SystemTestCollection")]
    public class CommunityE2ETests : PageTest
    {
        private readonly string BaseUrl;

        public CommunityE2ETests(Infrastructure.TestHost<Program> host)
        {
            BaseUrl = host.BaseUrl;
        }

        [Fact]
        public async Task Community_Group_Interactions()
        {
            // Login
            await Page.GotoAsync($"{BaseUrl}/Account/Login");
            await Page.Locator("input[type='email']").FillAsync("user@film.de");
            await Page.Locator("input[type='password']").FillAsync("User123!");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Anmelden" }).Or(Page.Locator("button[type='submit']")).First.ClickAsync();
            await Expect(Page).Not.ToHaveURLAsync(new System.Text.RegularExpressions.Regex(".*/Account/Login.*"));

            await Page.GotoAsync($"{BaseUrl}/Group/Discovery");
            
            // Create a group
            await Page.GetByRole(AriaRole.Link, new() { Name = "Neue Gruppe erstellen" }).Or(Page.GetByRole(AriaRole.Link, new() { Name = "Create New Group" })).Or(Page.Locator("a[href*='Create']")).First.ClickAsync();
            await Page.Locator("#Name").FillAsync("Test Fan Group");
            await Page.Locator("#Description").FillAsync("A group for E2E testing.");
            await Page.Locator("form[action*='Create'] button[type='submit']").First.ClickAsync();

            await Expect(Page.Locator("h1")).ToContainTextAsync("Test Fan Group");

            // Post a thread
            await Page.GetByRole(AriaRole.Link, new() { Name = "Neuer Thread" }).Or(Page.GetByRole(AriaRole.Link, new() { Name = "New Thread" })).Or(Page.Locator("a[href*='CreateThread']")).First.ClickAsync();
            await Page.Locator("#Title").FillAsync("E2E Discussion");
            await Page.Locator("#Content").FillAsync("Something interesting here.");
            await Page.Locator("form[action*='CreateThread'] button[type='submit']").First.ClickAsync();

            await Expect(Page.Locator("h1")).ToContainTextAsync("E2E Discussion");

            // Post a comment
            await Page.Locator("textarea[name='Content']").Last.FillAsync("My E2E comment");
            await Page.Locator("form[action*='PostComment'] button[type='submit']").First.ClickAsync();

            await Expect(Page.Locator("text=My E2E comment")).ToBeVisibleAsync();
        }
    }
}
