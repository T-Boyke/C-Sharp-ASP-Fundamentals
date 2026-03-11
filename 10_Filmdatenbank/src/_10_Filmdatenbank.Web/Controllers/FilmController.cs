using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Verwalte die Filme in der Datenbank.
/// </summary>
/// <param name="context">Der Datenbankkontext für den Zugriff auf Filmdaten.</param>
[Authorize]
[Route("Movies")]
[Route("Movies/[action]")]
public class FilmController(ApplicationDbContext context) : Controller
{
    /// <summary>
    /// Zeigt eine Liste aller Filme an.
    /// </summary>
    /// <returns>Die Index-View mit einer Liste von Filmen.</returns>
    public async Task<IActionResult> Index()
    {
        var filme = await context.Filme
            .Include(f => f.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Person)
            .Include(f => f.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Eigenschaft)
            .OrderBy(f => f.Titel)
            .ToListAsync();
        return View(filme);
    }

    /// <summary>
    /// Zeigt die Details eines bestimmten Films an.
    /// </summary>
    /// <param name="id">Die ID des Films.</param>
    /// <returns>Die Details-View des Films oder NotFound.</returns>
    public async Task<IActionResult> Details(int id)
    {
        var film = await context.Filme
            .Include(f => f.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Person)
            .Include(f => f.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Eigenschaft)
            .FirstOrDefaultAsync(f => f.FilmID == id);

        return film == null ? NotFound() : View(film);
    }

    /// <summary>
    /// Zeigt das Formular zum Erstellen eines neuen Films an.
    /// </summary>
    /// <returns>Die Create-View.</returns>
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    /// <summary>
    /// Erstellt einen neuen Film in der Datenbank.
    /// </summary>
    /// <param name="film">Das zu erstellende Film-Objekt.</param>
    /// <returns>Redirect zur Index-View bei Erfolg, andernfalls die Create-View mit Fehlern.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Film film)
    {
        if (ModelState.IsValid)
        {
            context.Filme.Add(film);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(film);
    }

    /// <summary>
    /// Zeigt das Formular zum Bearbeiten eines bestehenden Films an.
    /// </summary>
    /// <param name="id">Die ID des zu bearbeitenden Films.</param>
    /// <returns>Die Edit-View oder NotFound.</returns>
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var film = await context.Filme.FindAsync(id);
        return film == null ? NotFound() : View(film);
    }

    /// <summary>
    /// Aktualisiert einen bestehenden Film in der Datenbank.
    /// </summary>
    /// <param name="film">Das aktualisierte Film-Objekt.</param>
    /// <returns>Redirect zur Index-View bei Erfolg, andernfalls die Edit-View mit Fehlern.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Film film)
    {
        if (ModelState.IsValid)
        {
            context.Update(film);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(film);
    }

    /// <summary>
    /// Zeigt die Bestätigungsseite zum Löschen eines Films an.
    /// </summary>
    /// <param name="id">Die ID des zu löschenden Films.</param>
    /// <returns>Die Delete-View oder NotFound.</returns>
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var film = await context.Filme.FindAsync(id);
        return film == null ? NotFound() : View(film);
    }

    /// <summary>
    /// Bestätigt das Löschen eines Films und entfernt ihn aus der Datenbank.
    /// </summary>
    /// <param name="id">Die ID des zu löschenden Films.</param>
    /// <returns>Redirect zur Index-View.</returns>
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var film = await context.Filme.FindAsync(id);
        if (film != null)
        {
            context.Filme.Remove(film);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
