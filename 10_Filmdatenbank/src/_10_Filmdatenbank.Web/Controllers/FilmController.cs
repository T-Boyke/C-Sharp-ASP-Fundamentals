using _10_Filmdatenbank.Application.Interfaces;
using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Verwalte die Filme in der Datenbank.
/// </summary>
[Authorize]
[Route("Movies")]
[Route("Movies/[action]")]
public class FilmController(ApplicationDbContext context, ITmdbService tmdbService, ITvdbService tvdbService, UserManager<ApplicationUser> userManager) : Controller
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

        var userId = userManager.GetUserId(User);
        var favoriteFilmIds = userId != null 
            ? await context.FavoriteFilms.Where(ff => ff.UserID == userId).Select(ff => ff.FilmID).ToListAsync()
            : new List<int>();

        ViewBag.FavoriteFilmIds = favoriteFilmIds;

        var filme = await query
            .Include(f => f.Genres)
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
            .Include(f => f.FilmAwards)
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
    public async Task<IActionResult> Create(Film film, string? SelectedCastJson = null, string? SelectedCrewJson = null, string? SelectedGenresJson = null, string? SelectedKeywordsJson = null, string? SelectedCompaniesJson = null, int? TmdbCollectionId = null, string? TmdbCollectionName = null)
    {
        // Wir entfernen die Validierung für PersonEigenschaftFilme, da diese erst nach dem Film-Save verknüpft werden
        ModelState.Remove(nameof(film.PersonEigenschaftFilme));
        ModelState.Remove(nameof(film.Genres));
        ModelState.Remove(nameof(film.Keywords));
        ModelState.Remove(nameof(film.ProductionCompanies));
        ModelState.Remove(nameof(film.Collection));

        if (film == null) return BadRequest();

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
                    var colDetails = await tmdbService.GetCollectionDetailsAsync(TmdbCollectionId.Value);
                    collection = new Collection
                    {
                        TmdbId = TmdbCollectionId.Value,
                        Name = TmdbCollectionName,
                        Overview = colDetails?.Overview,
                        PosterUrl = string.IsNullOrEmpty(colDetails?.PosterPath) ? null : $"https://image.tmdb.org/t/p/w500{colDetails.PosterPath}",
                        BackdropUrl = string.IsNullOrEmpty(colDetails?.BackdropPath) ? null : $"https://image.tmdb.org/t/p/original{colDetails.BackdropPath}"
                    };
                    context.Collections.Add(collection);
                    await context.SaveChangesAsync();
                }
                
                if (collection != null)
                {
                    film.CollectionID = collection.CollectionID;
                }
            }

            // Automate enriched metadata from TMDB if TmdbId is present
            if (film.TmdbId.HasValue)
            {
                await HandleEnrichedFilmMetadataAsync(film, tmdbService, context);
            }

            context.Filme.Add(film);
            await context.SaveChangesAsync();
            TempData["Success"] = $"Film '{film.Titel}' wurde erfolgreich erstellt.";

            // Handle Genres
            if (!string.IsNullOrEmpty(SelectedGenresJson))
            {
                var genreData = System.Text.Json.JsonSerializer.Deserialize<List<GenreDto>>(SelectedGenresJson);
                if (genreData != null)
                {
                    foreach (var item in genreData)
                    {
                        var genre = context.Genres.Local.FirstOrDefault(g => g.TmdbId == item.Id)
                                    ?? await context.Genres.FirstOrDefaultAsync(g => g.TmdbId == item.Id)
                                    ?? await context.Genres.FirstOrDefaultAsync(g => g.Name == item.Name);
                        
                        if (genre == null)
                        {
                            genre = new Genre { Name = item.Name, TmdbId = item.Id };
                            context.Genres.Add(genre);
                        }
                        
                        if (!film.Genres.Any(g => g.TmdbId == item.Id))
                        {
                            film.Genres.Add(genre);
                        }
                    }
                }
            }

            // Handle Keywords
            if (!string.IsNullOrEmpty(SelectedKeywordsJson))
            {
                var keywordData = System.Text.Json.JsonSerializer.Deserialize<List<KeywordDto>>(SelectedKeywordsJson);
                if (keywordData != null)
                {
                    foreach (var item in keywordData)
                    {
                        var keyword = context.Keywords.Local.FirstOrDefault(k => k.TmdbId == item.Id)
                                      ?? await context.Keywords.FirstOrDefaultAsync(k => k.TmdbId == item.Id)
                                      ?? await context.Keywords.FirstOrDefaultAsync(k => k.Name == item.Name);
                        
                        if (keyword == null)
                        {
                            keyword = new Keyword { Name = item.Name, TmdbId = item.Id };
                            context.Keywords.Add(keyword);
                        }
                        
                        if (!film.Keywords.Any(k => k.TmdbId == item.Id))
                        {
                            film.Keywords.Add(keyword);
                        }
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
                        var company = context.ProductionCompanies.Local.FirstOrDefault(c => c.TmdbId == item.Id)
                                      ?? await context.ProductionCompanies.FirstOrDefaultAsync(c => c.TmdbId == item.Id)
                                      ?? await context.ProductionCompanies.FirstOrDefaultAsync(c => c.Name == item.Name);
                        
                        if (company == null)
                        {
                            var studio = await tmdbService.GetCompanyDetailsAsync(item.Id);
                            company = new ProductionCompany
                            {
                                Name = item.Name,
                                TmdbId = item.Id,
                                LogoUrl = string.IsNullOrEmpty(studio?.LogoPath) ? item.LogoUrl : $"https://image.tmdb.org/t/p/w500{studio.LogoPath}",
                                OriginCountry = studio?.OriginCountry ?? item.OriginCountry,
                                Headquarters = studio?.Headquarters,
                                Homepage = studio?.Homepage,
                                Description = studio?.Description
                            };
                            context.ProductionCompanies.Add(company);
                        }
                        
                        if (!film.ProductionCompanies.Any(c => c.TmdbId == item.Id))
                        {
                            film.ProductionCompanies.Add(company);
                        }
                    }
                }
            }

            // Handle Cast & Crew Synchronization
            await HandleCastCrewSync(SelectedCastJson, SelectedCrewJson, film, context, tmdbService);

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

    private async Task HandleEnrichedFilmMetadataAsync(Film film, ITmdbService tmdbService, ApplicationDbContext context)
    {
        if (!film.TmdbId.HasValue) return;

        var movie = await tmdbService.GetMovieDetailsAsync(film.TmdbId.Value);
        if (movie == null) return;

        // 1. Alternative Titles
        if (movie.AlternativeTitles?.Titles != null)
        {
            foreach (var alt in movie.AlternativeTitles.Titles)
            {
                if (!film.AlternativeTitles.Any(at => at.Title == alt.Title))
                {
                    film.AlternativeTitles.Add(new AlternativeTitle 
                    { 
                        Title = alt.Title, 
                        Iso3166_1 = alt.Iso_3166_1, 
                        Type = alt.Type 
                    });
                }
            }
        }

        // 2. Releases (Global Context)
        if (movie.ReleaseDates?.Results != null)
        {
            foreach (var res in movie.ReleaseDates.Results)
            {
                foreach (var release in res.ReleaseDates)
                {
                    if (!film.Releases.Any(r => r.Iso3166_1 == res.Iso_3166_1 && r.ReleaseDate == release.ReleaseDate))
                    {
                        film.Releases.Add(new FilmRelease
                        {
                            Iso3166_1 = res.Iso_3166_1,
                            Certification = release.Certification,
                            ReleaseDate = release.ReleaseDate,
                            Type = (int)release.Type,
                            Note = release.Note
                        });
                    }
                }
            }
        }

        // 3. Spoken Languages
        if (movie.SpokenLanguages != null)
        {
            foreach (var lang in movie.SpokenLanguages)
            {
                var dbLang = await context.Languages.FirstOrDefaultAsync(l => l.Iso639_1 == lang.Iso_639_1);
                if (dbLang == null)
                {
                    dbLang = new Language { Iso639_1 = lang.Iso_639_1, Name = lang.Name };
                    context.Languages.Add(dbLang);
                }
                
                if (!film.SpokenLanguages.Any(sl => sl.Iso639_1 == dbLang.Iso639_1))
                {
                    film.SpokenLanguages.Add(dbLang);
                }
            }
        }

        // 4. Production Countries
        if (movie.ProductionCountries != null)
        {
            foreach (var c in movie.ProductionCountries)
            {
                var dbCountry = await context.Countries.FirstOrDefaultAsync(co => co.Iso3166_1 == c.Iso_3166_1);
                if (dbCountry == null)
                {
                    dbCountry = new Country { Iso3166_1 = c.Iso_3166_1, Name = c.Name };
                    context.Countries.Add(dbCountry);
                }
                
                if (!film.ProductionCountries.Any(pc => pc.Iso3166_1 == dbCountry.Iso3166_1))
                {
                    film.ProductionCountries.Add(dbCountry);
                }
            }
        }

        // 5. TVDB ENRICHMENT
        if (!film.TvdbId.HasValue && !string.IsNullOrEmpty(movie.ExternalIds?.ImdbId))
        {
            // Search for TVDB ID using IMDB ID
            var tvdbResults = await tvdbService.SearchMoviesAsync(movie.ExternalIds.ImdbId);
            var tvdbMovie = tvdbResults.FirstOrDefault(r => r.PrimaryLanguage == "eng" || r.PrimaryLanguage == "deu");
            if (tvdbMovie != null && int.TryParse(tvdbMovie.TvdbId, out int tid))
            {
                film.TvdbId = tid;
            }
        }

        if (film.TvdbId.HasValue)
        {
            try
            {
                var tvdbDetails = await tvdbService.GetMovieExtendedDetailsAsync(film.TvdbId.Value);
                if (tvdbDetails != null)
                {
                    // Enrich with Awards from TVDB
                    if (tvdbDetails.Awards != null)
                    {
                        foreach (var award in tvdbDetails.Awards)
                        {
                            if (!film.FilmAwards.Any(fa => fa.Name == award.Name))
                            {
                                film.FilmAwards.Add(new FilmAward
                                {
                                    Name = award.Name,
                                    Year = award.Year,
                                    Category = award.Category,
                                    IsWinner = award.IsWinner
                                });
                            }
                        }
                    }

                    // Box Office / Budget if missing from TMDB
                    if ((film.Budget == null || film.Budget == 0) && tvdbDetails.Budget != null && long.TryParse(tvdbDetails.Budget, out long b))
                        film.Budget = b;
                    
                    if ((film.Revenue == null || film.Revenue == 0) && tvdbDetails.BoxOffice != null && long.TryParse(tvdbDetails.BoxOffice, out long r))
                        film.Revenue = r;

                    // Add more "Fanatic" info if available
                    // TVDB often has deeper tech specs or specialized lists
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TVDB Enrichment Error: {ex.Message}");
            }
        }
    }

    private async Task HandleCastCrewSync(string? castJson, string? crewJson, Film film, ApplicationDbContext context, ITmdbService tmdbService)
    {
        // 1. Handle Cast
        if (!string.IsNullOrEmpty(castJson))
        {
            try
            {
                var castItems = System.Text.Json.JsonSerializer.Deserialize<List<CastMemberDto>>(castJson);
                if (castItems != null)
                {
                    var actorProperty = await context.Eigenschaften.FirstOrDefaultAsync(e => e.Bezeichnung == "Actor")
                                        ?? new Eigenschaft { Bezeichnung = "Actor" };

                    if (actorProperty.EigenschaftID == 0) { context.Eigenschaften.Add(actorProperty); await context.SaveChangesAsync(); }

                    foreach (var item in castItems)
                    {
                        var person = await GetOrCreatePerson(item.id, item.name, item.profileUrl, context, tmdbService);
                        if (person != null && !film.PersonEigenschaftFilme.Any(pef => pef.PersonID == person.PersonID && pef.EigenschaftID == actorProperty.EigenschaftID))
                        {
                            context.PersonEigenschaftFilme.Add(new PersonEigenschaftFilm { FilmID = film.FilmID, PersonID = person.PersonID, EigenschaftID = actorProperty.EigenschaftID });
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Cast Sync Error: {ex.Message}"); }
        }

        // 2. Handle Crew
        if (!string.IsNullOrEmpty(crewJson))
        {
            try
            {
                var crewItems = System.Text.Json.JsonSerializer.Deserialize<List<CrewMemberDto>>(crewJson);
                if (crewItems != null)
                {
                    foreach (var item in crewItems)
                    {
                        var property = await context.Eigenschaften.FirstOrDefaultAsync(e => e.Bezeichnung == item.job)
                                       ?? new Eigenschaft { Bezeichnung = item.job };

                        if (property.EigenschaftID == 0) { context.Eigenschaften.Add(property); await context.SaveChangesAsync(); }

                        var person = await GetOrCreatePerson(item.id, item.name, item.profileUrl, context, tmdbService);
                        if (person != null && !film.PersonEigenschaftFilme.Any(pef => pef.PersonID == person.PersonID && pef.EigenschaftID == property.EigenschaftID))
                        {
                            context.PersonEigenschaftFilme.Add(new PersonEigenschaftFilm { FilmID = film.FilmID, PersonID = person.PersonID, EigenschaftID = property.EigenschaftID });
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Crew Sync Error: {ex.Message}"); }
        }
    }

    private async Task<Person?> GetOrCreatePerson(int tmdbId, string name, string? profileUrl, ApplicationDbContext context, ITmdbService tmdbService)
    {
        var person = await context.Personen.FirstOrDefaultAsync(p => p.TmdbId == tmdbId)
                     ?? await context.Personen.FirstOrDefaultAsync(p => (p.Vorname + " " + p.Nachname).Trim() == name.Trim());

        if (person == null)
        {
            var tmdbPerson = await tmdbService.GetPersonDetailsAsync(tmdbId);
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
                var names = name.Split(' ', 2);
                person = new Person
                {
                    Vorname = names[0],
                    Nachname = names.Length > 1 ? names[1] : string.Empty,
                    TmdbId = tmdbId,
                    ProfilBildUrl = profileUrl
                };
            }
            context.Personen.Add(person);
            await context.SaveChangesAsync();
        }
        return person;
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

    private class CrewMemberDto
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string job { get; set; } = string.Empty;
        public string? profileUrl { get; set; }
    }

    private class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private class KeywordDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
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

    [HttpPost]
    public async Task<IActionResult> ToggleFavorite(int filmId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var favorite = await context.FavoriteFilms
            .FirstOrDefaultAsync(ff => ff.FilmID == filmId && ff.UserID == user.Id);

        bool isAdded;
        string message;

        if (favorite != null)
        {
            context.FavoriteFilms.Remove(favorite);
            isAdded = false;
            message = "Vom Merkzettel entfernt.";
        }
        else
        {
            context.FavoriteFilms.Add(new FavoriteFilm
            {
                FilmID = filmId,
                UserID = user.Id,
                AddedAt = DateTime.UtcNow
            });
            isAdded = true;
            message = "Auf Merkzettel gespeichert.";
        }

        await context.SaveChangesAsync();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new { success = true, isAdded, message });
        }

        TempData[isAdded ? "Success" : "Info"] = message;
        return RedirectToAction(nameof(Details), new { id = filmId });
    }
}
