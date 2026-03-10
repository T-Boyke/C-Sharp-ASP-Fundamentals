using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace _08_Patienten2.Infrastructure.Identity;

/// <summary>
/// Stellt sicher, dass die Standard-Rollen und Admin-Accounts in der Datenbank existieren (RBAC).
/// </summary>
public static class EnsureIdentity
{
    public const string AdminRole = "Admin";
    public const string UserRole = "User";

    public static async Task SeedDefaultAccountsAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Rollen erstellen
        string[] roles = [AdminRole, UserRole];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Admin Account
        var adminEmail = "admin@praxis.de";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(adminUser, "Excellence2026!");
            await userManager.AddToRoleAsync(adminUser, AdminRole);
        }

        // Standard User Account
        var userEmail = "user@praxis.de";
        var standardUser = await userManager.FindByEmailAsync(userEmail);
        if (standardUser == null)
        {
            standardUser = new IdentityUser { UserName = userEmail, Email = userEmail, EmailConfirmed = true };
            await userManager.CreateAsync(standardUser, "Patient123!");
            await userManager.AddToRoleAsync(standardUser, UserRole);
        }
    }
}
