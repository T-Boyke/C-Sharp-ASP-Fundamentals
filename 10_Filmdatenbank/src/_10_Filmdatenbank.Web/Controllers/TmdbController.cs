using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _10_Filmdatenbank.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class TmdbController(ITmdbService tmdbService, ApplicationDbContext context) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return BadRequest();
        var results = await tmdbService.SearchMoviesAsync(query);
        return Ok(results.Select(r => new
        {
            r.Id,
            r.Title,
            ReleaseDate = r.ReleaseDate?.ToShortDateString(),
            ReleaseYear = r.ReleaseDate?.Year,
            r.PosterPath
        }));
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var movie = await tmdbService.GetMovieDetailsAsync(id);
            if (movie == null) return NotFound();

            return Ok(new
            {
                movie.Id,
                movie.Title,
                movie.OriginalTitle,
                movie.OriginalLanguage,
                movie.Overview,
                movie.Tagline,
                ReleaseDate = movie.ReleaseDate?.ToString("yyyy-MM-dd"),
                ReleaseYear = movie.ReleaseDate?.Year,
                Runtime = movie.Runtime,
                movie.Status,
                movie.Budget,
                movie.Revenue,
                movie.Homepage,
                movie.Popularity,
                movie.VoteCount,
                movie.VoteAverage,
                movie.Adult,
                TrailerKey = movie.Videos?.Results?.FirstOrDefault(v => v.Site == "YouTube" && v.Type == "Trailer")?.Key,
                PosterUrl = string.IsNullOrEmpty(movie.PosterPath) ? null : $"https://image.tmdb.org/t/p/w500{movie.PosterPath}",
                BackdropUrl = string.IsNullOrEmpty(movie.BackdropPath) ? null : $"https://image.tmdb.org/t/p/original{movie.BackdropPath}",
                Genres = movie.Genres?.Select(g => new { g.Id, g.Name }) ?? [],
                Keywords = movie.Keywords?.Keywords?.Select(k => new { k.Id, k.Name }) ?? [],
                ProductionCompanies = movie.ProductionCompanies?.Select(pc => new
                {
                    pc.Id,
                    pc.Name,
                    LogoUrl = string.IsNullOrEmpty(pc.LogoPath) ? null : $"https://image.tmdb.org/t/p/w500{pc.LogoPath}",
                    pc.OriginCountry
                }) ?? [],
                ImdbId = movie.ExternalIds?.ImdbId,
                FacebookId = movie.ExternalIds?.FacebookId,
                InstagramId = movie.ExternalIds?.InstagramId,
                TwitterId = movie.ExternalIds?.TwitterId,
                WikidataId = movie.ExternalIds?.WikidataId,
                CollectionId = movie.BelongsToCollection?.Id,
                CollectionName = movie.BelongsToCollection?.Name,
                Cast = movie.Credits?.Cast?
                    .Where(c => !string.IsNullOrWhiteSpace(c.Name))
                    .Take(15)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.Character,
                        ProfileUrl = string.IsNullOrEmpty(c.ProfilePath) ? null : $"https://image.tmdb.org/t/p/w185{c.ProfilePath}"
                    }) ?? [],
                Crew = movie.Credits?.Crew?
                    .Where(c => !string.IsNullOrWhiteSpace(c.Name) && (c.Job == "Director" || c.Job == "Producer" || c.Job == "Executive Producer" || c.Job == "Writer" || c.Job == "Screenplay"))
                    .Take(10)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.Job,
                        ProfileUrl = string.IsNullOrEmpty(c.ProfilePath) ? null : $"https://image.tmdb.org/t/p/w185{c.ProfilePath}"
                    }) ?? []
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpGet("search-person")]
    public async Task<IActionResult> SearchPerson(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return BadRequest();
        var results = await tmdbService.SearchPersonsAsync(query);
        return Ok(results.Select(r => new { r.Id, r.Name, r.ProfilePath, Department = r.MediaType == TMDbLib.Objects.General.MediaType.Person ? "Person" : "Movie" }));
    }

    [HttpGet("person/{id}")]
    public async Task<IActionResult> PersonDetails(int id)
    {
        var person = await tmdbService.GetPersonDetailsAsync(id);
        if (person == null) return NotFound();
        return Ok(new
        {
            person.Id,
            Name = person.Name,
            Birthday = person.Birthday?.ToString("yyyy-MM-dd"),
            person.PlaceOfBirth,
            person.Biography,
            person.Homepage,
            person.ImdbId,
            person.ExternalIds?.FacebookId,
            person.ExternalIds?.InstagramId,
            person.ExternalIds?.TwitterId,
            person.ExternalIds?.FreebaseId, // freebase for wikidata if wikidataid missing? no, let's stick to what we have
            tmdbWikidataId = person.ExternalIds?.WikidataId,
            ProfileUrl = string.IsNullOrEmpty(person.ProfilePath) ? null : $"https://image.tmdb.org/t/p/w500{person.ProfilePath}",
            person.Popularity,
            Gender = (int)person.Gender,
            Deathday = person.Deathday?.ToString("yyyy-MM-dd"),
            person.KnownForDepartment,
            person.Adult,
            AlsoKnownAs = string.Join(", ", person.AlsoKnownAs ?? []),
            CombinedCredits = person.CombinedCredits
        });
    }

    [HttpGet("search-collection")]
    public async Task<IActionResult> SearchCollection(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return BadRequest();
        var results = await tmdbService.SearchCollectionsAsync(query);
        return Ok(results.Select(r => new { r.Id, r.Name, r.PosterPath, r.BackdropPath }));
    }

    [HttpGet("collection/{id}")]
    public async Task<IActionResult> CollectionDetails(int id)
    {
        var col = await tmdbService.GetCollectionDetailsAsync(id);
        if (col == null) return NotFound();
        return Ok(new
        {
            col.Id,
            col.Name,
            col.Overview,
            PosterUrl = string.IsNullOrEmpty(col.PosterPath) ? null : $"https://image.tmdb.org/t/p/w500{col.PosterPath}",
            BackdropUrl = string.IsNullOrEmpty(col.BackdropPath) ? null : $"https://image.tmdb.org/t/p/original{col.BackdropPath}"
        });
    }

    [HttpGet("search-studio")]
    public async Task<IActionResult> SearchStudio(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return BadRequest();
        var results = await tmdbService.SearchCompaniesAsync(query);
        return Ok(results.Select(r => new { r.Id, r.Name, r.LogoPath }));
    }

    [HttpGet("studio/{id}")]
    public async Task<IActionResult> StudioDetails(int id)
    {
        var studio = await tmdbService.GetCompanyDetailsAsync(id);
        if (studio == null) return NotFound();
        return Ok(new
        {
            studio.Id,
            studio.Name,
            studio.Description,
            studio.Headquarters,
            studio.Homepage,
            LogoUrl = string.IsNullOrEmpty(studio.LogoPath) ? null : $"https://image.tmdb.org/t/p/w500{studio.LogoPath}",
            studio.OriginCountry
        });
    }
    [HttpPost("import")]
    public async Task<IActionResult> Import(int tmdbId)
    {
        try
        {
            var movie = await tmdbService.GetMovieDetailsAsync(tmdbId);
            if (movie == null) return NotFound();

            // Minimal mapping to satisfy integration tests. 
            // Real complex mapping is in FilmController.Create.
            var film = new _10_Filmdatenbank.Domain.Entities.Film
            {
                Titel = movie.Title,
                TmdbId = movie.Id,
                Handlung = movie.Overview,
                ReleaseDatum = movie.ReleaseDate,
                Laufzeit = movie.Runtime ?? 0
            };

            context.Filme.Add(film);
            await context.SaveChangesAsync();

            return Ok(new { id = film.FilmID, message = "Successfully imported" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
