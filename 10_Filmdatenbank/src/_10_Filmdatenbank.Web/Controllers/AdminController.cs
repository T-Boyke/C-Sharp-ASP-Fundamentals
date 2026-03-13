using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Controller for administrative tasks including IAM and RBAC.
/// </summary>
[Authorize(Roles = "Admin")]
public class AdminController(UserManager<IdentityUser> userManager) : Controller
{
    /// <summary>
    /// Displays the profile edit page (IAM).
    /// </summary>
    /// <returns>The profile edit view.</returns>
    public IActionResult EditProfile()
    {
        return View();
    }

    /// <summary>
    /// Displays the user management page (RBAC).
    /// </summary>
    /// <returns>A list of users.</returns>
    public async Task<IActionResult> ManageUsers()
    {
        var users = await userManager.Users.ToListAsync();
        return View(users);
    }
}
