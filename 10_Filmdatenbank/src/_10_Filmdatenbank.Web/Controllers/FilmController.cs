using _10_Filmdatenbank.Application.Interfaces;
using _10_Filmdatenbank.Domain.Entities;
using _10_Filmdatenbank.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMDbLib.Objects.Search;

namespace _10_Filmdatenbank.Web.Controllers;

/// <summary>
/// Verwalte die Filme in der Datenbank.
/// </summary>
[Authorize]
[Route("Movies")]
[Route("Movies/[action]")]
public class FilmController(ApplicationDbContext context, ITmdbService tmdbService, ITvdbService tvdbService, IRottenTomatoesService rtService, IImdbService imdbService, IMetacriticService metacriticService, IWikidataService wikidataService, UserManager<ApplicationUser> userManager) : Controller
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
            : [];

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
            .Include(f => f.MetacriticReviews)
            .Include(f => f.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Person)
            .Include(f => f.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Eigenschaft)
            .Include(f => f.FilmAwards)
            .FirstOrDefaultAsync(f => f.FilmID == id);

        var userId = userManager.GetUserId(User);
        if (userId != null)
        {
            var userRating = await context.UserRatings.FirstOrDefaultAsync(r => r.FilmID == id && r.UserID == userId);
            ViewBag.CurrentUserRating = userRating?.Value;
        }

        return film == null ? NotFound() : View(film);
    }

    /// <summary>
    /// Ermöglicht es einem Benutzer, einen Film zu bewerten (0-10 Punkte).
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rate(int id, double rating)
    {
        var userId = userManager.GetUserId(User);
        if (userId == null) return Challenge();

        if (rating < 0 || rating > 10) return BadRequest("Rating must be between 0 and 10.");

        var film = await context.Filme.Include(f => f.UserRatings).FirstOrDefaultAsync(f => f.FilmID == id);
        if (film == null) return NotFound();

        var existingRating = await context.UserRatings.FirstOrDefaultAsync(r => r.FilmID == id && r.UserID == userId);
        if (existingRating != null)
        {
            existingRating.Value = rating;
            existingRating.CreatedAt = DateTime.UtcNow;
            context.UserRatings.Update(existingRating);
        }
        else
        {
            context.UserRatings.Add(new UserRating
            {
                FilmID = id,
                UserID = userId,
                Value = rating
            });
        }

        await context.SaveChangesAsync();

        // Update Cached Average
        var ratings = await context.UserRatings.Where(r => r.FilmID == id).ToListAsync();
        film.Nutzerwertung = ratings.Average(r => r.Value);
        film.CouchDbVoteCount = ratings.Count;

        await context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Bewertung erfolgreich gespeichert!";
        return RedirectToAction(nameof(Details), new { id });
    }

    /// <summary>
    /// Zeigt das globale Ranking der Filme basierend auf der CouchDB Nutzerwertung an.
    /// </summary>
    public async Task<IActionResult> Ranking()
    {
        var films = await context.Filme
            .Include(f => f.Genres)
            .Where(f => f.Nutzerwertung != null && f.CouchDbVoteCount > 0)
            .OrderByDescending(f => f.Nutzerwertung)
            .ThenByDescending(f => f.CouchDbVoteCount)
            .Take(100)
            .ToListAsync();

        return View(films);
    }

    /// <summary>
    /// Synchronisiert externe Bewertungen (RT, IMDb Metascore) für einen Film.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SyncExternalScores(int id)
    {
        var film = await context.Filme
            .Include(f => f.Genres)
            .Include(f => f.Keywords)
            .Include(f => f.ProductionCountries)
            .Include(f => f.SpokenLanguages)
            .Include(f => f.Releases)
            .Include(f => f.AlternativeTitles)
            .Include(f => f.ProductionCompanies)
            .Include(f => f.PersonEigenschaftFilme)
                .ThenInclude(pef => pef.Person)
            .Include(f => f.FilmAwards)
            .Include(f => f.MetacriticReviews)
            .FirstOrDefaultAsync(f => f.FilmID == id);
        if (film == null) return NotFound();

        var updated = false;

        // Fully sync with all metadata sources
        try
        {
            await HandleEnrichedFilmMetadataAsync(film, tmdbService, context);
            updated = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Full Sync Error: {ex.Message}");
        }

        if (updated)
        {
            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Externe Scores erfolgreich synchronisiert!";
        }
        else
        {
            TempData["ErrorMessage"] = "Keine neuen Daten gefunden.";
        }

        return RedirectToAction(nameof(Details), new { id });
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

        // 0. Base Stats (Safe mapping with 0-10 normalization)
        film.VoteAverage = movie.VoteAverage > 100 ? movie.VoteAverage / 1000.0 : (movie.VoteAverage > 10 ? movie.VoteAverage / 10.0 : movie.VoteAverage);
        film.VoteCount = movie.VoteCount;
        film.Popularity = movie.Popularity;
        film.Budget = movie.Budget;
        film.Revenue = movie.Revenue;
        film.Runtime = movie.Runtime;
        film.OriginalLanguage = movie.OriginalLanguage;
        film.OriginalTitle = movie.OriginalTitle;
        film.Status = movie.Status;
        film.Tagline = movie.Tagline;
        film.Handlung = movie.Overview;
        film.PosterUrl ??= string.IsNullOrEmpty(movie.PosterPath) ? null : $"https://image.tmdb.org/t/p/w500{movie.PosterPath}";
        film.BackdropUrl ??= string.IsNullOrEmpty(movie.BackdropPath) ? null : $"https://image.tmdb.org/t/p/original{movie.BackdropPath}";

        var trailer = movie.Videos?.Results?.FirstOrDefault(v => v.Site == "YouTube" && v.Type == "Trailer");
        if (trailer != null && string.IsNullOrEmpty(film.TrailerUrl))
        {
            film.TrailerUrl = $"https://www.youtube.com/embed/{trailer.Key}";
        }

        // 1. Alternative Titles
        if (movie.AlternativeTitles?.Titles != null)
        {
            foreach (var alt in movie.AlternativeTitles.Titles)
            {
                if (!film.AlternativeTitles.Any(at => at.Title == alt.Title))
                {
                    film.AlternativeTitles.Add(new AlternativeTitle
                    {
                        Title = alt.Title ?? "Unknown",
                        Iso3166_1 = alt.Iso_3166_1 ?? "",
                        Type = alt.Type ?? ""
                    });
                }
            }
        }

        // 2. Releases (Global Context)
        if (movie.ReleaseDates?.Results != null)
        {
            foreach (var res in movie.ReleaseDates.Results)
            {
                foreach (var release in res.ReleaseDates ?? [])
                {
                    if (!film.Releases.Any(r => r.Iso3166_1 == res.Iso_3166_1 && r.ReleaseDate == release.ReleaseDate))
                    {
                        film.Releases.Add(new FilmRelease
                        {
                            Iso3166_1 = res.Iso_3166_1 ?? "",
                            Certification = release.Certification ?? "",
                            ReleaseDate = release.ReleaseDate,
                            Type = (int)release.Type,
                            Note = release.Note ?? ""
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
                var dbLang = context.Languages.Local.FirstOrDefault(l => l.Iso639_1 == lang.Iso_639_1)
                                ?? await context.Languages.FirstOrDefaultAsync(l => l.Iso639_1 == lang.Iso_639_1);
                if (dbLang == null)
                {
                    dbLang = new Language { Iso639_1 = lang.Iso_639_1 ?? "??", Name = lang.Name ?? "Unknown" };
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
                var dbCountry = context.Countries.Local.FirstOrDefault(co => co.Iso3166_1 == c.Iso_3166_1)
                                ?? await context.Countries.FirstOrDefaultAsync(co => co.Iso3166_1 == c.Iso_3166_1);
                if (dbCountry == null)
                {
                    dbCountry = new Country { Iso3166_1 = c.Iso_3166_1 ?? "", Name = c.Name ?? c.Iso_3166_1 ?? "Unknown" };
                    context.Countries.Add(dbCountry);
                }

                if (!film.ProductionCountries.Any(pc => pc.Iso3166_1 == dbCountry.Iso3166_1))
                {
                    film.ProductionCountries.Add(dbCountry);
                }
            }
        }

        // 5. Genres
        if (movie.Genres != null)
        {
            foreach (var g in movie.Genres)
            {
                var dbGenre = context.Genres.Local.FirstOrDefault(genre => genre.TmdbId == g.Id)
                              ?? await context.Genres.FirstOrDefaultAsync(genre => genre.TmdbId == g.Id)
                              ?? await context.Genres.FirstOrDefaultAsync(genre => genre.Name == g.Name);

                if (dbGenre == null)
                {
                    dbGenre = new Genre { Name = g.Name ?? "Unknown", TmdbId = g.Id };
                    context.Genres.Add(dbGenre);
                }

                if (!film.Genres.Any(fg => fg.TmdbId == g.Id))
                {
                    film.Genres.Add(dbGenre);
                }
            }
        }

        // 6. Keywords
        if (movie.Keywords?.Keywords != null)
        {
            foreach (var k in movie.Keywords.Keywords)
            {
                var dbKeyword = context.Keywords.Local.FirstOrDefault(kw => kw.TmdbId == k.Id)
                                ?? await context.Keywords.FirstOrDefaultAsync(kw => kw.TmdbId == k.Id)
                                ?? await context.Keywords.FirstOrDefaultAsync(kw => kw.Name == k.Name);

                if (dbKeyword == null)
                {
                    dbKeyword = new Keyword { Name = k.Name ?? "Unknown", TmdbId = k.Id };
                    context.Keywords.Add(dbKeyword);
                }

                if (!film.Keywords.Any(fk => fk.TmdbId == k.Id))
                {
                    film.Keywords.Add(dbKeyword);
                }
            }
        }

        // 7. Production Companies
        if (movie.ProductionCompanies != null)
        {
            foreach (var pc in movie.ProductionCompanies)
            {
                var dbCompany = context.ProductionCompanies.Local.FirstOrDefault(c => c.TmdbId == pc.Id)
                                ?? await context.ProductionCompanies.FirstOrDefaultAsync(c => c.TmdbId == pc.Id)
                                ?? await context.ProductionCompanies.FirstOrDefaultAsync(c => c.Name == pc.Name);

                if (dbCompany == null)
                {
                    var studio = await tmdbService.GetCompanyDetailsAsync(pc.Id);
                    dbCompany = new ProductionCompany
                    {
                        Name = pc.Name ?? "Unknown",
                        TmdbId = pc.Id,
                        LogoUrl = string.IsNullOrEmpty(studio?.LogoPath) ? null : $"https://image.tmdb.org/t/p/w500{studio.LogoPath}",
                        OriginCountry = studio?.OriginCountry ?? pc.OriginCountry,
                        Headquarters = studio?.Headquarters,
                        Homepage = studio?.Homepage,
                        Description = studio?.Description
                    };
                    context.ProductionCompanies.Add(dbCompany);
                }

                if (!film.ProductionCompanies.Any(fc => fc.TmdbId == pc.Id))
                {
                    film.ProductionCompanies.Add(dbCompany);
                }
            }
        }

        // 8. TVDB ENRICHMENT
        if (!film.TvdbId.HasValue && !string.IsNullOrEmpty(movie.ExternalIds?.ImdbId))
        {
            // Search for TVDB ID using IMDB ID
            var tvdbRemoteResults = await tvdbService.GetByRemoteIdAsync(movie.ExternalIds.ImdbId);
            var tvdbMovieResult = tvdbRemoteResults.FirstOrDefault(r => r.Movie != null);
            if (tvdbMovieResult?.Movie?.Id != null)
            {
                film.TvdbId = (int)tvdbMovieResult.Movie.Id.Value;
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
                                    Year = int.TryParse(tvdbDetails.Year, out var y) ? y : (film.Erscheinungsdatum?.Year ?? 0),
                                    Category = "Imported (TVDB)",
                                    IsWin = false // TVDB record here only contains basic info
                                });
                            }
                        }
                    }

                    // Box Office / Budget if missing from TMDB
                    if ((film.Budget == null || film.Budget == 0) && tvdbDetails.Budget != null && long.TryParse(tvdbDetails.Budget, out var b))
                        film.Budget = b;

                    if ((film.Revenue == null || film.Revenue == 0) && tvdbDetails.BoxOffice != null && long.TryParse(tvdbDetails.BoxOffice, out var r))
                        film.Revenue = r;

                    // 1. Aliases mapping
                    if (tvdbDetails.Aliases != null)
                    {
                        foreach (var alias in tvdbDetails.Aliases)
                        {
                            if (!film.AlternativeTitles.Any(at => at.Title == alias.Name))
                            {
                                film.AlternativeTitles.Add(new AlternativeTitle
                                {
                                    Title = alias.Name,
                                    Iso3166_1 = alias.Language ?? "XX",
                                    Type = "TVDB Alias"
                                });
                            }
                        }
                    }

                    // 2. Inspirations (Books, real events, etc.)
                    if (tvdbDetails.Inspirations != null && tvdbDetails.Inspirations.Count > 0)
                    {
                        var notes = tvdbDetails.Inspirations.Select(i => $"{i.Type_name}: {i.Url}").ToList();
                        film.ProductionNotes = string.Join("; ", notes);
                    }

                    if (tvdbDetails.Score != null)
                    {
                        // TVDB Score normalization: Logic depends on the range. 
                        // If it's a huge popularity number (e.g. 127000), we don't use it as rating.
                        // If it's in a reasonable range (0-100 or 0-10), we normalize it to 10.
                        double score = tvdbDetails.Score.Value;
                        if (score > 1000)
                        {
                            // This is likely a popularity score (e.g. 127000), not a rating. 
                            // We don't want to show this as /10 in the UI.
                            film.TvdbRating = null; 
                        }
                        else
                        {
                            // Map 0-100 to 0-10
                            film.TvdbRating = score > 10 ? score / 10.0 : score;
                        }
                    }

                    // Add more "Fanatic" info if available
                    // TVDB often has deeper tech specs or specialized lists
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TVDB Enrichment Error: {ex.Message}");
            }
        }

        // 9. Watch Providers (Streaming) - Priority Region "DE"
        if (movie.WatchProviders?.Results != null && movie.WatchProviders.Results.TryGetValue("DE", out var deObj))
        {
            var de = (dynamic)deObj;
            context.WatchProviders.RemoveRange(film.WatchProviders);
            film.WatchProviders.Clear();

            if (de.FlatRate != null)
                foreach (var p in de.FlatRate) film.WatchProviders.Add(new WatchProvider { Name = p.ProviderName, LogoUrl = $"https://image.tmdb.org/t/p/original{p.LogoPath}", Type = "flatrate", DisplayPriority = p.DisplayPriority ?? 0 });

            if (de.Rent != null)
                foreach (var p in de.Rent) film.WatchProviders.Add(new WatchProvider { Name = p.ProviderName, LogoUrl = $"https://image.tmdb.org/t/p/original{p.LogoPath}", Type = "rent", DisplayPriority = p.DisplayPriority ?? 0 });

            if (de.Buy != null)
                foreach (var p in de.Buy) film.WatchProviders.Add(new WatchProvider { Name = p.ProviderName, LogoUrl = $"https://image.tmdb.org/t/p/original{p.LogoPath}", Type = "buy", DisplayPriority = p.DisplayPriority ?? 0 });
        }

        // 10. Commercial Resources (Amazon / Physical Media)
        if (film.ExternalResources.Count == 0 || !film.ExternalResources.Any(r => r.Type == "Amazon"))
        {
            film.ExternalResources.Add(new ExternalResource
            {
                Type = "Amazon",
                Label = "Amazon Store (DE)",
                Url = $"https://www.amazon.de/s?k={Uri.EscapeDataString(film.Titel)}+Blu-ray"
            });
        }

        // 11. Local Media (Placeholder / NAS Detection Logic)
        if (string.IsNullOrEmpty(film.LocalNasPath))
        {
            // Future logic: Check file system for matches
            // film.LocalNasPath = $"\\\\NAS\\Movies\\{film.Titel} ({film.Erscheinungsjahr}).mkv";
        }

        // 🌀 Rotten Tomatoes Enrichment (Special Logic)
        try
        {
            var releaseYear = film.Erscheinungsdatum?.Year;
            var rtHit = await rtService.SearchMovieAsync(film.Titel, releaseYear);
            if (rtHit?.Scores != null)
            {
                film.RottenTomatoesCriticRating = rtHit.Scores.CriticsScore;
                film.RottenTomatoesAudienceRating = rtHit.Scores.AudienceScore;
                Console.WriteLine($"RT Enrichment Success: {film.Titel} -> {film.RottenTomatoesCriticRating}%/{film.RottenTomatoesAudienceRating}%");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"RT Enrichment Error: {ex.Message}");
        }

        // 🎞️ IMDb & Metacritic Rating Enrichment (Shared Source)
        if (!string.IsNullOrEmpty(film.ImdbId))
        {
            try
            {
                var imdbMeta = await imdbService.GetMetadataAsync(film.ImdbId);
                if (imdbMeta != null)
                {
                    if (imdbMeta.Rating.HasValue)
                        film.ImdbRating = imdbMeta.Rating.Value;

                    if (imdbMeta.Metascore.HasValue)
                        film.MetacriticRating = imdbMeta.Metascore.Value;

                    Console.WriteLine($"IMDb/MC Enrichment Success: {film.Titel} -> IMDb: {film.ImdbRating}, Metascore: {film.MetacriticRating}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IMDb Enrichment Error: {ex.Message}");
            }
        }

        // 🧪 Metacritic Deep Scraping (Expert Reviews & User Scores)
        try
        {
            var mcData = await metacriticService.GetDeepMetadataAsync(film.Titel, film.Erscheinungsdatum?.Year);
            if (mcData != null)
            {
                if (mcData.Metascore.HasValue) film.MetacriticRating = mcData.Metascore.Value;
                if (mcData.UserScore.HasValue) film.MetacriticUserScore = mcData.UserScore.Value;
                if (!string.IsNullOrEmpty(mcData.MetacriticUrl)) film.MetacriticUrl = mcData.MetacriticUrl;

                // Sync Reviews
                context.MetacriticReviews.RemoveRange(film.MetacriticReviews);
                film.MetacriticReviews.Clear();

                foreach (var r in mcData.Reviews)
                {
                    film.MetacriticReviews.Add(new MetacriticReview
                    {
                        Author = r.Author,
                        Publication = r.Publication,
                        Score = r.Score,
                        Content = r.Snippet ?? "",
                        ReviewUrl = r.ReviewUrl
                    });
                }
                Console.WriteLine($"Metacritic Deep Enrichment Success: {film.Titel} -> {film.MetacriticReviews.Count} reviews");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Metacritic Deep Enrichment Error: {ex.Message}");
        }

        // 🏛️ Wikidata Enrichment (Studios & People)
        try
        {
            await EnrichPeopleWithWikidataAsync(film);
            await EnrichStudiosWithWikidataAsync(film);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wikidata Enrichment Global Error: {ex.Message}");
        }
    }

    private async Task EnrichPeopleWithWikidataAsync(Film film)
    {
        if (film.PersonEigenschaftFilme == null || film.PersonEigenschaftFilme.Count == 0) return;

        foreach (var pef in film.PersonEigenschaftFilme)
        {
            var person = pef.Person;
            if (person == null) continue;

            // Only enrich if we have an ID and data is missing
            if ((!string.IsNullOrEmpty(person.WikidataId) || !string.IsNullOrEmpty(person.ImdbId)) && (string.IsNullOrEmpty(person.Geburtsort) || string.IsNullOrEmpty(person.InstagramId)))
            {
                try
                {
                    var wikiData = await wikidataService.GetPersonDetailsAsync(person.WikidataId, person.ImdbId);
                    if (wikiData != null)
                    {
                        if (string.IsNullOrEmpty(person.Geburtsort)) person.Geburtsort = wikiData.BirthPlace;
                        if (string.IsNullOrEmpty(person.Tags)) person.Tags = wikiData.ZodiacSign; // Using Tags for Zodiac

                        if (string.IsNullOrEmpty(person.Biografie) && !string.IsNullOrEmpty(wikiData.Description))
                            person.Biografie = wikiData.Description;

                        person.InstagramId ??= wikiData.InstagramId;
                        person.TwitterId ??= wikiData.TwitterId;
                        person.FacebookId ??= wikiData.FacebookId;

                        // Append Awards if not present
                        if (person.PersonAwards != null && person.PersonAwards.Count == 0 && wikiData.Awards.Count > 0)
                        {
                            foreach (var wa in wikiData.Awards)
                            {
                                person.PersonAwards.Add(new PersonAward
                                {
                                    Name = wa.Name,
                                    Category = wa.Category,
                                    Year = wa.Year,
                                    IsWin = wa.IsWin
                                });
                            }
                        }

                        Console.WriteLine($"Wikidata Person Success: {person.Vorname} {person.Nachname}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wikidata Person Error ({person.Nachname}): {ex.Message}");
                }
            }
        }
    }

    private async Task EnrichStudiosWithWikidataAsync(Film film)
    {
        if (film.ProductionCompanies == null || film.ProductionCompanies.Count == 0) return;

        foreach (var company in film.ProductionCompanies)
        {
            // Try to enrich if we have at least one ID
            if (!string.IsNullOrEmpty(company.WikidataId) || company.TmdbId > 0)
            {
                try
                {
                    var wikiData = await wikidataService.GetCompanyDetailsAsync(company.WikidataId, company.TmdbId);
                    if (wikiData != null)
                    {
                        company.Headquarters ??= wikiData.Headquarters;
                        company.FoundedYear ??= wikiData.FoundedYear;
                        company.EmployeeCount ??= wikiData.EmployeeCount;
                        // We could also store the parent company name in description if needed
                        if (!string.IsNullOrEmpty(wikiData.ParentCompany) && !string.IsNullOrEmpty(company.Description) && !company.Description.Contains(wikiData.ParentCompany))
                        {
                            company.Description += $" (Teil von {wikiData.ParentCompany})";
                        }

                        Console.WriteLine($"Wikidata Studio Success: {company.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wikidata Studio Error ({company.Name}): {ex.Message}");
                }
            }
        }
    }

    private static async Task HandleCastCrewSync(string? castJson, string? crewJson, Film film, ApplicationDbContext context, ITmdbService tmdbService)
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
                        var person = await GetOrCreatePerson(item.Id, item.Name, item.ProfileUrl, context, tmdbService);
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
                        var property = await context.Eigenschaften.FirstOrDefaultAsync(e => e.Bezeichnung == item.Job)
                                       ?? new Eigenschaft { Bezeichnung = item.Job };

                        if (property.EigenschaftID == 0) { context.Eigenschaften.Add(property); await context.SaveChangesAsync(); }

                        var person = await GetOrCreatePerson(item.Id, item.Name, item.ProfileUrl, context, tmdbService);
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

    private static async Task<Person?> GetOrCreatePerson(int tmdbId, string name, string? profileUrl, ApplicationDbContext context, ITmdbService tmdbService)
    {
        var person = await context.Personen.FirstOrDefaultAsync(p => p.TmdbId == tmdbId)
                     ?? await context.Personen.FirstOrDefaultAsync(p => (p.Vorname + " " + p.Nachname).Trim() == name.Trim());

        if (person == null)
        {
            var tmdbPerson = await tmdbService.GetPersonDetailsAsync(tmdbId);
            if (tmdbPerson != null)
            {
                var names = (tmdbPerson.Name ?? "Unknown").Split(' ', 2);
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
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public string? ProfileUrl { get; set; }
    }

    private class CrewMemberDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Job { get; set; } = string.Empty;
        public string? ProfileUrl { get; set; }
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
    public async Task<IActionResult> Edit(Film film, string? SelectedCastJson = null, string? SelectedCrewJson = null, string? SelectedGenresJson = null, string? SelectedKeywordsJson = null, string? SelectedCompaniesJson = null, int? TmdbCollectionId = null, string? TmdbCollectionName = null)
    {
        ModelState.Remove(nameof(film.PersonEigenschaftFilme));
        ModelState.Remove(nameof(film.Genres));
        ModelState.Remove(nameof(film.Keywords));
        ModelState.Remove(nameof(film.ProductionCompanies));
        ModelState.Remove(nameof(film.Collection));

        if (ModelState.IsValid)
        {
            var dbFilm = await context.Filme
                .Include(f => f.Genres)
                .Include(f => f.Keywords)
                .Include(f => f.ProductionCompanies)
                .Include(f => f.PersonEigenschaftFilme)
                .Include(f => f.Collection)
                .FirstOrDefaultAsync(f => f.FilmID == film.FilmID);

            if (dbFilm == null) return NotFound();

            // Update scalar properties
            context.Entry(dbFilm).CurrentValues.SetValues(film);

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
                dbFilm.CollectionID = collection?.CollectionID;
            }

            // Handle Genres
            if (!string.IsNullOrEmpty(SelectedGenresJson))
            {
                var genreData = System.Text.Json.JsonSerializer.Deserialize<List<GenreDto>>(SelectedGenresJson);
                if (genreData != null)
                {
                    dbFilm.Genres.Clear();
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
                        dbFilm.Genres.Add(genre);
                    }
                }
            }

            // Handle Keywords
            if (!string.IsNullOrEmpty(SelectedKeywordsJson))
            {
                var keywordData = System.Text.Json.JsonSerializer.Deserialize<List<KeywordDto>>(SelectedKeywordsJson);
                if (keywordData != null)
                {
                    dbFilm.Keywords.Clear();
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
                        dbFilm.Keywords.Add(keyword);
                    }
                }
            }

            // Handle Production Companies
            if (!string.IsNullOrEmpty(SelectedCompaniesJson))
            {
                var companies = System.Text.Json.JsonSerializer.Deserialize<List<CompanyDto>>(SelectedCompaniesJson);
                if (companies != null)
                {
                    dbFilm.ProductionCompanies.Clear();
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
                        dbFilm.ProductionCompanies.Add(company);
                    }
                }
            }

            // Handle Cast & Crew
            if (SelectedCastJson != null || SelectedCrewJson != null)
            {
                // We clear old relations for simplicity in Edit or we could merge. 
                // Let's merge or follow Create logic. Create logic doesn't clear because it's new.
                // In Edit, syncing usually means replacing the cast with the selection.
                context.PersonEigenschaftFilme.RemoveRange(dbFilm.PersonEigenschaftFilme);
                await HandleCastCrewSync(SelectedCastJson, SelectedCrewJson, dbFilm, context, tmdbService);
            }

            await context.SaveChangesAsync();
            TempData["Success"] = "Änderungen wurden erfolgreich gespeichert.";
            return RedirectToAction(nameof(Details), new { id = dbFilm.FilmID });
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

        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            return Json(new { success = true, isAdded, message });
        }

        TempData[isAdded ? "Success" : "Info"] = message;
        return RedirectToAction(nameof(Details), new { id = filmId });
    }
}
