using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using _10_Filmdatenbank.Application.Interfaces;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Controller für die Verwaltung von Filmkollektionen (Filmreihen).
/// </summary>
[Authorize]
[Route("Kollektionen")]
[Route("Kollektionen/[action]")]
public class CollectionController(ApplicationDbContext context, ITmdbService tmdbService) : Controller
{
    public async Task<IActionResult> Index(string? searchString)
    {
        var query = context.Collections.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(c => c.Name.Contains(searchString) || (c.Overview != null && c.Overview.Contains(searchString)));
            ViewData["CurrentFilter"] = searchString;
        }

        var collections = await query
            .Include(c => c.Films)
            .OrderBy(c => c.Name)
            .ToListAsync();
        return View(collections);
    }

    public async Task<IActionResult> Details(int id)
    {
        var collection = await context.Collections
            .Include(c => c.Films)
            .FirstOrDefaultAsync(c => c.CollectionID == id);

        if (collection == null) return NotFound();

        // Fetch total parts from TMDB for Mastery tracking
        if (collection.TmdbId > 0)
        {
            var tmdbCol = await tmdbService.GetCollectionDetailsAsync(collection.TmdbId);
            ViewData["TotalParts"] = tmdbCol?.Parts?.Count ?? collection.Films.Count;
        }

        return View(collection);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Collection collection)
    {
        if (ModelState.IsValid)
        {
            context.Collections.Add(collection);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(collection);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var collection = await context.Collections.FindAsync(id);
        return collection == null ? NotFound() : View(collection);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Collection collection)
    {
        if (ModelState.IsValid)
        {
            context.Update(collection);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(collection);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var collection = await context.Collections.FindAsync(id);
        return collection == null ? NotFound() : View(collection);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var collection = await context.Collections.FindAsync(id);
        if (collection != null)
        {
            context.Collections.Remove(collection);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
