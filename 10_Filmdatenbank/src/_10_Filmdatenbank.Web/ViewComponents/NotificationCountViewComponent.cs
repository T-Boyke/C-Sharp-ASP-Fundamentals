using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using _10_Filmdatenbank.Domain.Entities;

namespace _10_Filmdatenbank.Web.ViewComponents;

public class NotificationCountViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public NotificationCountViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = _userManager.GetUserId(HttpContext.User);
        if (userId == null) return View(0);

        var count = await _context.Notifications
            .Where(n => n.UserID == userId && !n.IsRead)
            .CountAsync();

        return View(count);
    }
}
