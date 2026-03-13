using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
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
}
