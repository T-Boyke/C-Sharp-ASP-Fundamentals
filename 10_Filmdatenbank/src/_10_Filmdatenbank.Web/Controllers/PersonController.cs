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
    /// <param name="id">Die ID der Person.</param>
    /// <returns>Die Details-View der Person oder NotFound.</returns>
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
}
