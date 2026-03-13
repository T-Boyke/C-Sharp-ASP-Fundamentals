using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace _10_Filmdatenbank.Web.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Dashboard()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Challenge();

        var user = await _context.Users
            .Include(u => u.Notifications)
            .Include(u => u.GroupMemberships).ThenInclude(m => m.FanGroup)
            .Include(u => u.Threads)
            .Include(u => u.FavoriteFilms).ThenInclude(ff => ff.Film)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return View(user);
    }

    public async Task<IActionResult> Profile()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Challenge();

        var user = await _context.Users
            .Include(u => u.FavoriteFilms).ThenInclude(ff => ff.Film)
            .FirstOrDefaultAsync(u => u.Id == userId);
            
        return View(user);
    }

    public async Task<IActionResult> Groups()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var memberships = await _context.GroupMembers
            .Include(m => m.FanGroup)
            .Where(m => m.UserID == user.Id)
            .ToListAsync();

        return View(memberships);
    }
}
