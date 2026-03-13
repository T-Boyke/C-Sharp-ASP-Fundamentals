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
/// <param name="tmdbService">Der Dienst für den Zugriff auf TMDB-Daten.</param>
[Authorize]
[Route("Movies")]
[Route("Movies/[action]")]
public class FilmController(ApplicationDbContext context, _10_Filmdatenbank.Application.Interfaces.ITmdbService tmdbService) : Controller
{
    /// <summary>
    /// Zeigt eine Liste aller Filme an.
    /// </summary>
    /// <param name="searchString">Optionaler Suchbegriff.</param>
    /// <returns>Die Index-View mit einer Liste von Filmen.</returns>
    public async Task<IActionResult> Index(string? searchString = null)
    {
        var query = context.Filme.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(f => f.Titel.Contains(searchString) || (f.Handlung != null && f.Handlung.Contains(searchString)));
            ViewData["CurrentFilter"] = searchString;
        }

        var filme = await query
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
            .Include(f => f.Genres)
            .Include(f => f.Keywords)
            .Include(f => f.ProductionCompanies)
            .Include(f => f.Collection)
            .Include(f => f.Releases)
            .Include(f => f.ProductionCountries)
            .Include(f => f.SpokenLanguages)
            .Include(f => f.AlternativeTitles)
            .Include(f => f.SimilarFilms)
            .Include(f => f.RecommendedFilms)
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
    public async Task<IActionResult> Create(Film film, string? SelectedCastJson = null, string? SelectedGenresJson = null, string? SelectedKeywordsJson = null, string? SelectedCompaniesJson = null, int? TmdbCollectionId = null, string? TmdbCollectionName = null)
    {
        // Wir entfernen die Validierung für PersonEigenschaftFilme, da diese erst nach dem Film-Save verknüpft werden
        ModelState.Remove(nameof(film.PersonEigenschaftFilme));
        ModelState.Remove(nameof(film.Genres));
        ModelState.Remove(nameof(film.Keywords));
        ModelState.Remove(nameof(film.ProductionCompanies));
        ModelState.Remove(nameof(film.Collection));

        if (film.TmdbId.HasValue && await context.Filme.AnyAsync(f => f.TmdbId == film.TmdbId))
        {
            var msg = "Dieser Film existiert bereits in der Datenbank.";
            ModelState.AddModelError("", msg);
            TempData["Error"] = msg;
        }

        if (ModelState.IsValid)
        {
            // Handle Collection Synchronization
            if (TmdbCollectionId.HasValue)
            {
                var collection = await context.Collections.FirstOrDefaultAsync(c => c.TmdbId == TmdbCollectionId.Value);
                if (collection == null && !string.IsNullOrEmpty(TmdbCollectionName))
                {
                    collection = new Collection
                    {
                        TmdbId = TmdbCollectionId.Value,
                        Name = TmdbCollectionName
                    };
                    context.Collections.Add(collection);
                    await context.SaveChangesAsync();
                }
                
                if (collection != null)
                {
                    film.CollectionID = collection.CollectionID;
                }
            }

            context.Filme.Add(film);
            await context.SaveChangesAsync();
            TempData["Success"] = $"Film '{film.Titel}' wurde erfolgreich erstellt.";

            // Handle Genres
            if (!string.IsNullOrEmpty(SelectedGenresJson))
            {
                var genreNames = System.Text.Json.JsonSerializer.Deserialize<List<string>>(SelectedGenresJson);
                if (genreNames != null)
                {
                    foreach (var name in genreNames)
                    {
                        var genre = await context.Genres.FirstOrDefaultAsync(g => g.Name == name)
                                    ?? new Genre { Name = name };
                        film.Genres.Add(genre);
                    }
                }
            }

            // Handle Keywords
            if (!string.IsNullOrEmpty(SelectedKeywordsJson))
            {
                var keywordNames = System.Text.Json.JsonSerializer.Deserialize<List<string>>(SelectedKeywordsJson);
                if (keywordNames != null)
                {
                    foreach (var name in keywordNames)
                    {
                        var keyword = await context.Keywords.FirstOrDefaultAsync(k => k.Name == name)
                                      ?? new Keyword { Name = name };
                        film.Keywords.Add(keyword);
                    }
                }
            }

            // Handle Production Companies
            if (!string.IsNullOrEmpty(SelectedCompaniesJson))
            {
                var companies = System.Text.Json.JsonSerializer.Deserialize<List<CompanyDto>>(SelectedCompaniesJson);
                if (companies != null)
                {
                    foreach (var item in companies)
                    {
                        var company = await context.ProductionCompanies.FirstOrDefaultAsync(c => c.TmdbId == item.Id)
                                      ?? await context.ProductionCompanies.FirstOrDefaultAsync(c => c.Name == item.Name);
                        
                        if (company == null)
                        {
                            company = new ProductionCompany
                            {
                                Name = item.Name,
                                TmdbId = item.Id,
                                LogoUrl = item.LogoUrl,
                                OriginCountry = item.OriginCountry
                            };
                            context.ProductionCompanies.Add(company);
                        }
                        film.ProductionCompanies.Add(company);
                    }
                }
            }

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
                                // Fetch full person details from TMDB for enrichment
                                var tmdbPerson = await tmdbService.GetPersonDetailsAsync(item.id);
                                if (tmdbPerson != null)
                                {
                                    var names = tmdbPerson.Name.Split(' ', 2);
                                    person = new Person
                                    {
                                        Vorname = names[0],
                                        Nachname = names.Length > 1 ? names[1] : string.Empty,
                                        TmdbId = tmdbPerson.Id,
                                        ProfilBildUrl = string.IsNullOrEmpty(tmdbPerson.ProfilePath) ? null : $"https://image.tmdb.org/t/p/w500{tmdbPerson.ProfilePath}",
                                        Biografie = tmdbPerson.Biography,
                                        Geburtsdatum = tmdbPerson.Birthday,
                                        Geburtsort = tmdbPerson.PlaceOfBirth,
                                        Gender = (int)tmdbPerson.Gender,
                                        Deathday = tmdbPerson.Deathday,
                                        Homepage = tmdbPerson.Homepage,
                                        Popularity = tmdbPerson.Popularity,
                                        ImdbId = tmdbPerson.ImdbId,
                                        KnownForDepartment = tmdbPerson.KnownForDepartment,
                                        Adult = tmdbPerson.Adult,
                                        AlsoKnownAs = string.Join(", ", tmdbPerson.AlsoKnownAs ?? []),
                                        FacebookId = tmdbPerson.ExternalIds?.FacebookId,
                                        InstagramId = tmdbPerson.ExternalIds?.InstagramId,
                                        TwitterId = tmdbPerson.ExternalIds?.TwitterId,
                                        WikidataId = tmdbPerson.ExternalIds?.WikidataId,
                                        FreebaseId = tmdbPerson.ExternalIds?.FreebaseId,
                                        FreebaseMid = tmdbPerson.ExternalIds?.FreebaseMid,
                                        TvrageId = tmdbPerson.ExternalIds?.TvrageId,
                                        TmdbFilmographyJson = System.Text.Json.JsonSerializer.Serialize(tmdbPerson.CombinedCredits)
                                    };
                                }
                                else
                                {
                                    // Fallback to basic info if detail fetch fails
                                    var names = item.name.Split(' ', 2);
                                    person = new Person
                                    {
                                        Vorname = names[0],
                                        Nachname = names.Length > 1 ? names[1] : string.Empty,
                                        TmdbId = item.id,
                                        ProfilBildUrl = item.profileUrl,
                                        Biografie = $"Automatischer Import von TMDB. Charakter: {item.character}"
                                    };
                                }
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
                    }
                }
                catch (Exception ex)
                {
                    // Log error but allow film creation to proceed
                    Console.WriteLine($"Cast Sync Error: {ex.Message}");
                }
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Log validation errors for debugging
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine($"Validation Error: {error.ErrorMessage}");
        }

        return View(film);
    }

    private class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? OriginCountry { get; set; }
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
