using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _10_Filmdatenbank.Web.Controllers;

[Authorize]
public class GroupController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Discovery()
    {
        var groups = await context.FanGroups
            .Include(g => g.Members)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
        return View(groups);
    }

    public async Task<IActionResult> Details(int id)
    {
        var group = await context.FanGroups
            .Include(g => g.Members).ThenInclude(m => m.User)
            .Include(g => g.Threads).ThenInclude(t => t.Author)
            .FirstOrDefaultAsync(g => g.FanGroupID == id);

        if (group == null) return NotFound();

        return View(group);
    }

    [HttpPost]
    public async Task<IActionResult> Join(int id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var existing = await context.GroupMembers
            .AnyAsync(m => m.FanGroupID == id && m.UserID == user.Id);

        if (existing) return RedirectToAction(nameof(Details), new { id });

        var group = await context.FanGroups.FindAsync(id);
        if (group == null) return NotFound();

        if (group.RequiresApproval)
        {
            context.MembershipRequests.Add(new MembershipRequest
            {
                FanGroupID = id,
                UserID = user.Id,
                Status = RequestStatus.Pending
            });
            TempData["Info"] = "Beitrittsanfrage gesendet.";
        }
        else
        {
            context.GroupMembers.Add(new GroupMember
            {
                FanGroupID = id,
                UserID = user.Id,
                Role = GroupRole.Member
            });
            TempData["Success"] = $"Willkommen in der Gruppe '{group.Name}'!";
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    public async Task<IActionResult> Leave(int id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var membership = await context.GroupMembers
            .FirstOrDefaultAsync(m => m.FanGroupID == id && m.UserID == user.Id);

        if (membership != null)
        {
            context.GroupMembers.Remove(membership);
            await context.SaveChangesAsync();
            TempData["Info"] = "Gruppe verlassen.";
        }

        return RedirectToAction(nameof(Discovery));
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FanGroup group, Microsoft.AspNetCore.Http.IFormFile? ImageFile)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        if (ModelState.IsValid)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                using var ms = new System.IO.MemoryStream();
                await ImageFile.CopyToAsync(ms);
                group.GroupImage = ms.ToArray();
                group.GroupImageContentType = ImageFile.ContentType;
            }

            group.CreatedAt = DateTime.UtcNow;
            context.FanGroups.Add(group);
            await context.SaveChangesAsync();

            // Add creator as Owner
            context.GroupMembers.Add(new GroupMember
            {
                FanGroupID = group.FanGroupID,
                UserID = user.Id,
                Role = GroupRole.Owner,
                JoinedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
            TempData["Success"] = $"Gruppe '{group.Name}' erfolgreich erstellt!";
            return RedirectToAction(nameof(Details), new { id = group.FanGroupID });
        }

        return View(group);
    }
}
