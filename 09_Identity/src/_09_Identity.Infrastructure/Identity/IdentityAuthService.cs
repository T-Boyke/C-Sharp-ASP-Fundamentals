using Microsoft.AspNetCore.Identity;
using _09_Identity.Domain.Interfaces;
using _09_Identity.Domain.ValueObjects;

namespace _09_Identity.Infrastructure.Identity;

/// <summary>
/// Implementierung des IAuthService unter Verwendung von ASP.NET Core Identity.
/// </summary>
public class IdentityAuthService(
    UserManager<IdentityUser> userManager, 
    SignInManager<IdentityUser> signInManager) : IAuthService
{
    public async Task<bool> LoginAsync(Credentials credentials)
    {
        var user = await userManager.FindByNameAsync(credentials.Username);
        if (user == null) return false;

        await signInManager.SignOutAsync();
        var result = await signInManager.PasswordSignInAsync(user, credentials.Password, false, false);
        
        return result.Succeeded;
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}
