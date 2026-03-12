using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Controller für die Verwaltung und Anzeige von Personen (Schauspieler, Regisseure etc.).
/// </summary>
/// <param name="context">Der Datenbankkontext für den Zugriff auf Personendaten.</param>
[Authorize]
[Route("Schauspieler")]
[Route("Schauspieler/[action]")]
public class PersonController(ApplicationDbContext context) : Controller
{
    /// <summary>
    /// Zeigt eine Liste aller Personen in der Datenbank an.
    /// </summary>
    /// <returns>Die Index-View mit einer Liste von Personen.</returns>
    public async Task<IActionResult> Index()
    {
        var personen = await context.Personen
            .Include(p => p.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Eigenschaft)
            .OrderBy(p => p.Nachname)
            .ToListAsync();
        return View(personen);
    }

    /// <summary>
    /// Zeigt die Details einer bestimmten Person an.
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        var person = await context.Personen
            .Include(p => p.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Film)
            .Include(p => p.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Eigenschaft)
            .FirstOrDefaultAsync(p => p.PersonID == id);

        if (person == null) return NotFound();

        return View(person);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(_10_Filmdatenbank.Domain.Entities.Person person)
    {
        if (ModelState.IsValid)
        {
            context.Personen.Add(person);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(person);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var person = await context.Personen.FindAsync(id);
        return person == null ? NotFound() : View(person);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(_10_Filmdatenbank.Domain.Entities.Person person)
    {
        if (ModelState.IsValid)
        {
            context.Update(person);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(person);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var person = await context.Personen.FindAsync(id);
        return person == null ? NotFound() : View(person);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var person = await context.Personen.FindAsync(id);
        if (person != null)
        {
            context.Personen.Remove(person);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
