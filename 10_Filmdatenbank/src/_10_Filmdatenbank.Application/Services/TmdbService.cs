using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace _10_Filmdatenbank.Application.Services;

public class TmdbService : ITmdbService
{
    private readonly TMDbClient _client;

    public TmdbService(IConfiguration configuration)
    {
        var apiKey = configuration["TMDB:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            // Fallback for user provided key if configuration is missing
            apiKey = "d32c6254aebfa68d0c01e5995711ffc1";
        }
        _client = new TMDbClient(apiKey);
    }

    public async Task<IEnumerable<SearchMovie>> SearchMoviesAsync(string query, string language = "de-DE")
    {
        var results = await _client.SearchMovieAsync(query, language: language);
        return results.Results;
    }

    public async Task<Movie> GetMovieDetailsAsync(int tmdbId, string language = "de-DE")
    {
        return await _client.GetMovieAsync(tmdbId, language: language, extraMethods: MovieMethods.Credits | MovieMethods.ExternalIds | MovieMethods.Keywords);
    }
}
