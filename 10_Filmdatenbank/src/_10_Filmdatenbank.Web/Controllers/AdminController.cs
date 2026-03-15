using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Controller for administrative tasks including IAM and RBAC.
/// </summary>
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer _localizer;

    public AdminController(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer localizer)
    {
        _userManager = userManager;
        _context = context;
        _localizer = localizer;
    }

    /// <summary>
    /// Displays the user management page (RBAC).
    /// </summary>
    public async Task<IActionResult> ManageUsers()
    {
        var users = await _userManager.Users
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();
        return View(users);
    }

    /// <summary>
    /// Displays the system settings and maintenance page.
    /// </summary>
    public IActionResult Settings()
    {
        return View();
    }

    /// <summary>
    /// High-risk maintenance operation: Drops the entire database and resets it to a clean state.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DropDatabase()
    {
        // 1. Drop the database
        await _context.Database.EnsureDeletedAsync();

        // 2. Re-apply migrations
        await _context.Database.MigrateAsync();

        // 3. Re-seed essential data (Roles and Admin)
        var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!await roleManager.RoleExistsAsync("Member")) await roleManager.CreateAsync(new IdentityRole("Member"));

        if (await userManager.FindByEmailAsync("admin@film.de") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@film.de",
                Email = "admin@film.de",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                CreatedAt = DateTime.UtcNow,
                IsDisabled = false
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Displays the group moderation page (COMM).
    /// </summary>
    public async Task<IActionResult> ManageGroups()
    {
        var groups = await _context.FanGroups
            .Include(g => g.Members)
            .Include(g => g.JoinRequests)
            .ToListAsync();
        return View(groups);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleUserStatus(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.IsDisabled = !user.IsDisabled;
            await _userManager.UpdateAsync(user);
        }
        return RedirectToAction(nameof(ManageUsers));
    }

    /// <summary>
    /// Displays the edit user page.
    /// </summary>
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    /// <summary>
    /// Updates the user information.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ApplicationUser user)
    {
        var dbUser = await _userManager.FindByIdAsync(user.Id);
        if (dbUser == null) return NotFound();

        dbUser.FirstName = user.FirstName;
        dbUser.LastName = user.LastName;
        dbUser.Email = user.Email;
        dbUser.UserName = user.Email; // Keep in sync for simplicity
        dbUser.IsDisabled = user.IsDisabled;

        var result = await _userManager.UpdateAsync(dbUser);
        if (result.Succeeded)
        {
            TempData["Success"] = $"User '{dbUser.Email}' updated successfully.";
            return RedirectToAction(nameof(ManageUsers));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(user);
    }
}
