using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _10_Filmdatenbank.Web.Components;

public class NotificationCountViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public NotificationCountViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userId == null)
        {
            return View(0);
        }

        var unreadCount = await _context.Notifications
            .Where(n => n.UserID == userId && !n.IsRead)
            .CountAsync();

        return View(unreadCount);
    }
}
