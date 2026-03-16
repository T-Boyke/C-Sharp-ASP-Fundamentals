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
public class PersonController(ApplicationDbContext context, IWikidataService wikidataService, ITmdbService tmdbService) : Controller
{
    /// <summary>
    /// Zeigt eine Liste aller Personen in der Datenbank an.
    /// </summary>
    /// <param name="searchString">Optionaler Suchbegriff.</param>
    /// <param name="role">Optionaler Filter nach Rolle (Bezeichnung).</param>
    /// <returns>Die Index-View mit einer Liste von Personen.</returns>
    public async Task<IActionResult> Index(string? searchString, string? role)
    {
        var query = context.Personen.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(p => p.Vorname.Contains(searchString) || p.Nachname.Contains(searchString) || (p.Biografie != null && p.Biografie.Contains(searchString)));
            ViewData["CurrentFilter"] = searchString;
        }

        if (!string.IsNullOrEmpty(role))
        {
            query = query.Where(p => p.PersonEigenschaftFilme.Any(pef => pef.Eigenschaft.Bezeichnung == role));
            ViewData["CurrentRole"] = role;
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

        // 🌀 External Data Enrichment (On Demand)
        if (!string.IsNullOrEmpty(person.WikidataId) || !string.IsNullOrEmpty(person.ImdbId) || person.TmdbId.HasValue)
        {
            await EnrichPersonAsync(person);
            await context.SaveChangesAsync();
        }

        return View(person);
    }

    private async Task EnrichPersonAsync(_10_Filmdatenbank.Domain.Entities.Person person)
    {
        // Enrich from TMDB if ID available (Fetch Filmography)
        if (person.TmdbId.HasValue)
        {
            try
            {
                var tmdbPerson = await tmdbService.GetPersonDetailsAsync(person.TmdbId.Value);
                if (tmdbPerson != null)
                {
                    // Update basic info if missing
                    person.ProfilBildUrl ??= string.IsNullOrEmpty(tmdbPerson.ProfilePath) ? null : $"https://image.tmdb.org/t/p/h632{tmdbPerson.ProfilePath}";
                    person.Biografie ??= tmdbPerson.Biography;
                    person.Geburtsdatum ??= tmdbPerson.Birthday;
                    person.Deathday ??= tmdbPerson.Deathday;
                    person.Homepage ??= tmdbPerson.Homepage;
                    person.Popularity ??= tmdbPerson.Popularity;
                    person.Gender ??= (int)tmdbPerson.Gender;
                    person.KnownForDepartment ??= tmdbPerson.KnownForDepartment;

                    if (tmdbPerson.ExternalIds != null)
                    {
                        person.ImdbId ??= tmdbPerson.ExternalIds.ImdbId;
                        person.FacebookId ??= tmdbPerson.ExternalIds.FacebookId;
                        person.InstagramId ??= tmdbPerson.ExternalIds.InstagramId;
                        person.TwitterId ??= tmdbPerson.ExternalIds.TwitterId;
                        person.FreebaseId ??= tmdbPerson.ExternalIds.FreebaseId;
                        person.TvrageId ??= tmdbPerson.ExternalIds.TvrageId;
                    }

                    // 🎬 Store Combined Credits as JSON for Global Filmography
                    if (tmdbPerson.CombinedCredits != null)
                    {
                        person.TmdbFilmographyJson = Newtonsoft.Json.JsonConvert.SerializeObject(tmdbPerson.CombinedCredits);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TMDB Person Enrichment Error: {ex.Message}");
            }
        }

        // Don't re-enrich from Wikidata if we already have structured awards and a bio
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
