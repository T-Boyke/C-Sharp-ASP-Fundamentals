using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using _10_Filmdatenbank.Web.Models;
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

    [HttpGet]
    public async Task<IActionResult> CreateThread(int groupId)
    {
        var group = await context.FanGroups.FindAsync(groupId);
        if (group == null) return NotFound();

        // Verify membership
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        var isMember = await context.GroupMembers
            .AnyAsync(m => m.FanGroupID == groupId && m.UserID == user.Id);

        if (!isMember)
        {
            TempData["Error"] = "Du musst Mitglied der Gruppe sein, um eine Diskussion zu starten.";
            return RedirectToAction(nameof(Details), new { id = groupId });
        }

        ViewBag.GroupName = group.Name;
        return View(new CreateThreadViewModel { FanGroupID = groupId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateThread(CreateThreadViewModel model)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        if (ModelState.IsValid)
        {
            var thread = new DiscussionThread
            {
                FanGroupID = model.FanGroupID,
                Title = model.Title,
                Content = model.Content,
                AuthorID = user.Id,
                CreatedAt = DateTime.UtcNow,
                LastActivity = DateTime.UtcNow
            };

            context.DiscussionThreads.Add(thread);
            await context.SaveChangesAsync();

            TempData["Success"] = "Diskussion erfolgreich erstellt!";
            return RedirectToAction(nameof(ThreadDetails), new { id = thread.ThreadID });
        }

        TempData["Error"] = "Diskussion konnte nicht erstellt werden. Bitte überprüfe deine Angaben.";
        var group = await context.FanGroups.FindAsync(model.FanGroupID);
        ViewBag.GroupName = group?.Name;
        return View(model);
    }

    public async Task<IActionResult> ThreadDetails(int id)
    {
        var thread = await context.DiscussionThreads
            .Include(t => t.Author)
            .Include(t => t.FanGroup)
            .Include(t => t.Comments).ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(t => t.ThreadID == id);

        if (thread == null) return NotFound();

        return View(thread);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostComment(Comment comment)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        if (string.IsNullOrWhiteSpace(comment.Content))
        {
            ModelState.AddModelError("", "Kommentar darf nicht leer sein.");
        }

        if (ModelState.IsValid)
        {
            comment.AuthorID = user.Id;
            comment.CreatedAt = DateTime.UtcNow;

            context.Comments.Add(comment);

            // Update last activity of thread
            var thread = await context.DiscussionThreads.FindAsync(comment.ThreadID);
            if (thread != null)
            {
                thread.LastActivity = DateTime.UtcNow;
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(ThreadDetails), new { id = comment.ThreadID });
        }

        return RedirectToAction(nameof(ThreadDetails), new { id = comment.ThreadID });
    }
}
