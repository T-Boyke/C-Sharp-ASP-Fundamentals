using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace _10_Filmdatenbank.Application.Interfaces;

public interface ITmdbService
{
    Task<IEnumerable<SearchMovie>> SearchMoviesAsync(string query, string language = "de-DE");
    Task<Movie> GetMovieDetailsAsync(int tmdbId, string language = "de-DE");
}
