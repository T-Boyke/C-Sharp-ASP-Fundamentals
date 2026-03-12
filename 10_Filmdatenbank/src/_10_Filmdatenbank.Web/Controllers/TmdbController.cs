using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _10_Filmdatenbank.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class TmdbController(ITmdbService tmdbService) : ControllerBase
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
            PosterUrl = string.IsNullOrEmpty(movie.PosterPath) ? null : $"https://image.tmdb.org/t/p/w500{movie.PosterPath}",
            BackdropUrl = string.IsNullOrEmpty(movie.BackdropPath) ? null : $"https://image.tmdb.org/t/p/original{movie.BackdropPath}",
            Genres = string.Join(", ", movie.Genres.Select(g => g.Name)),
            ImdbId = movie.ExternalIds?.ImdbId
        });
    }
}
