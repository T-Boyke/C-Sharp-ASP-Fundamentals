using _10_Filmdatenbank.Infrastructure.Persistence;
using _10_Filmdatenbank.Application.Interfaces;
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
public class PersonController(ApplicationDbContext context, IWikidataService wikidataService) : Controller
{
    /// <summary>
    /// Zeigt eine Liste aller Personen in der Datenbank an.
    /// </summary>
    /// <param name="searchString">Optionaler Suchbegriff.</param>
    /// <returns>Die Index-View mit einer Liste von Personen.</returns>
    public async Task<IActionResult> Index(string? searchString)
    {
        var query = context.Personen.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(p => p.Vorname.Contains(searchString) || p.Nachname.Contains(searchString) || (p.Biografie != null && p.Biografie.Contains(searchString)));
            ViewData["CurrentFilter"] = searchString;
        }

        var personen = await query
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
                    .ThenInclude(f => f.Genres)
            .Include(p => p.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Film)
                    .ThenInclude(f => f.PersonEigenschaftFilme)
                        .ThenInclude(pef => pef.Person)
            .Include(p => p.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Eigenschaft)
            .Include(p => p.PersonAwards)
            .FirstOrDefaultAsync(p => p.PersonID == id);

        if (person == null) return NotFound();

        // 🌀 Wikidata Enrichment (On Demand)
        if (!string.IsNullOrEmpty(person.WikidataId) || !string.IsNullOrEmpty(person.ImdbId))
        {
            await EnrichPersonAsync(person);
            await context.SaveChangesAsync();
        }

        return View(person);
    }

    private async Task EnrichPersonAsync(_10_Filmdatenbank.Domain.Entities.Person person)
    {
        // Don't re-enrich if we already have awards or a bio
        if (person.PersonAwards.Any() && !string.IsNullOrEmpty(person.Biografie)) return;

        try
        {
            var wikiData = await wikidataService.GetPersonDetailsAsync(person.WikidataId, person.ImdbId);
            if (wikiData != null)
            {
                // Update basic fields if empty
                if (string.IsNullOrEmpty(person.Geburtsort)) person.Geburtsort = wikiData.BirthPlace;
                if (string.IsNullOrEmpty(person.Tags)) person.Tags = wikiData.ZodiacSign;

                person.InstagramId ??= wikiData.InstagramId;
                person.TwitterId ??= wikiData.TwitterId;
                person.FacebookId ??= wikiData.FacebookId;

                // Enhanced Bio / Description
                if (string.IsNullOrEmpty(person.Biografie) && !string.IsNullOrEmpty(wikiData.Description))
                {
                    person.Biografie = wikiData.Description;
                }

                // Append Awards if not present
                if (!person.PersonAwards.Any() && wikiData.Awards.Any())
                {
                    foreach (var wa in wikiData.Awards)
                    {
                        person.PersonAwards.Add(new _10_Filmdatenbank.Domain.Entities.PersonAward
                        {
                            Name = wa.Name,
                            Category = wa.Category,
                            Year = wa.Year,
                            IsWin = wa.IsWin
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wikidata Person Enrichment Error: {ex.Message}");
        }
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
