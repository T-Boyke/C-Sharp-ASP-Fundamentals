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
    /// <param name="SelectedCastJson">JSON-String der ausgewählten Cast-Mitglieder.</param>
    /// <returns>Redirect zur Index-View bei Erfolg, andernfalls die Create-View mit Fehlern.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Film film, string? SelectedCastJson)
    {
        // Wir entfernen die Validierung für PersonEigenschaftFilme, da diese erst nach dem Film-Save verknüpft werden
        ModelState.Remove(nameof(film.PersonEigenschaftFilme));

        if (film.TmdbId.HasValue && await context.Filme.AnyAsync(f => f.TmdbId == film.TmdbId))
        {
            var msg = "Dieser Film existiert bereits in der Datenbank.";
            ModelState.AddModelError("", msg);
            TempData["Error"] = msg;
        }

        if (ModelState.IsValid)
        {
            context.Filme.Add(film);
            await context.SaveChangesAsync();
            TempData["Success"] = $"Film '{film.Titel}' wurde erfolgreich erstellt.";

            // Handle Cast Synchronization
            if (!string.IsNullOrEmpty(SelectedCastJson))
            {
                try
                {
                    var castItems = System.Text.Json.JsonSerializer.Deserialize<List<CastMemberDto>>(SelectedCastJson);
                    if (castItems != null && castItems.Any())
                    {
                        var actorProperty = await context.Eigenschaften.FirstOrDefaultAsync(e => e.Bezeichnung == "Actor")
                                            ?? new Eigenschaft { Bezeichnung = "Actor" };

                        if (actorProperty.EigenschaftID == 0)
                        {
                            context.Eigenschaften.Add(actorProperty);
                            await context.SaveChangesAsync();
                        }

                        foreach (var item in castItems)
                        {
                            // Smart Lookup: TmdbId first, then name match
                            var person = await context.Personen.FirstOrDefaultAsync(p => p.TmdbId == item.id)
                                         ?? await context.Personen.FirstOrDefaultAsync(p => (p.Vorname + " " + p.Nachname).Trim() == item.name.Trim());

                            if (person == null)
                            {
                                var names = item.name.Split(' ', 2);
                                person = new Person
                                {
                                    Vorname = names[0],
                                    Nachname = names.Length > 1 ? names[1] : string.Empty,
                                    TmdbId = item.id,
                                    ProfilBildUrl = item.profileUrl,
                                    Biografie = $"Automatischer Import von TMDB. Charakter: {item.character}"
                                };
                                context.Personen.Add(person);
                                await context.SaveChangesAsync();
                            }

                            var pef = new PersonEigenschaftFilm
                            {
                                FilmID = film.FilmID,
                                PersonID = person.PersonID,
                                EigenschaftID = actorProperty.EigenschaftID
                            };
                            context.PersonEigenschaftFilme.Add(pef);
                        }
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    // Log error but allow film creation to proceed
                    Console.WriteLine($"Cast Sync Error: {ex.Message}");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // Log validation errors for debugging
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine($"Validation Error: {error.ErrorMessage}");
        }

        return View(film);
    }

    private class CastMemberDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string character { get; set; } = string.Empty;
        public string? profileUrl { get; set; }
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
            TempData["Success"] = "Änderungen wurden gespeichert.";
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
    public async Task<IActionResult> DeleteConfirmed(int FilmID)
    {
        var film = await context.Filme.FindAsync(FilmID);
        if (film != null)
        {
            var titel = film.Titel;
            context.Filme.Remove(film);
            await context.SaveChangesAsync();
            TempData["Success"] = $"Film '{titel}' wurde gelöscht.";
        }
        return RedirectToAction(nameof(Index));
    }
}
