using _10_Filmdatenbank.Application.Models.RottenTomatoes;

namespace _10_Filmdatenbank.Application.Interfaces;

/// <summary>
/// Service for interacting with Rotten Tomatoes via their search API.
/// </summary>
public interface IRottenTomatoesService
{
    /// <summary>
    /// Searches for a movie on Rotten Tomatoes and returns its scores.
    /// </summary>
    /// <param name="title">The title of the movie.</param>
    /// <param name="year">The release year (optional, to improve matching).</param>
    /// <returns>The scores and metadata if found; otherwise null.</returns>
    Task<RottenTomatoesHit?> SearchMovieAsync(string title, int? year = null);
}
