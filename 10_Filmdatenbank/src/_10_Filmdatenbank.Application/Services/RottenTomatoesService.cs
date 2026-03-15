using System.Net.Http.Json;
using System.Text.Json;
using _10_Filmdatenbank.Application.Interfaces;
using _10_Filmdatenbank.Application.Models.RottenTomatoes;
using Microsoft.Extensions.Logging;

namespace _10_Filmdatenbank.Application.Services;

/// <summary>
/// Implementation of IRottenTomatoesService using Algolia Search API.
/// </summary>
public class RottenTomatoesService : IRottenTomatoesService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RottenTomatoesService> _logger;

    private const string AlgoliaAppId = "79FRDP12PN";
    private const string AlgoliaApiKey = "175588f6e5f8319b27702e4cc4013561";
    private const string SearchUrl = $"https://{AlgoliaAppId}-dsn.algolia.net/1/indexes/*/queries?x-algolia-agent=Algolia%20for%20JavaScript%20(4.26.0)%3B%20Browser%20(lite)&x-algolia-application-id={AlgoliaAppId}&x-algolia-api-key={AlgoliaApiKey}";

    public RottenTomatoesService(HttpClient httpClient, ILogger<RottenTomatoesService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<RottenTomatoesHit?> SearchMovieAsync(string title, int? year = null)
    {
        try
        {
            var request = new AlgoliaSearchRequest
            {
                Requests = new List<AlgoliaRequestItem>
                {
                    new AlgoliaRequestItem
                    {
                        Query = title
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync(SearchUrl, request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<AlgoliaSearchResponse>();
            if (content?.Results == null || !content.Results.Any())
            {
                return null;
            }

            var hits = content.Results.SelectMany(r => r.Hits).ToList();

            // Filter for movies and best match
            var movieHit = hits
                .Where(h => h.Type.Equals("movie", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(h => h.ReleaseYear == year) // Prefer exact year
                .ThenByDescending(h => h.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (movieHit != null)
            {
                _logger.LogInformation("Found Rotten Tomatoes match for {Title} ({Year}): {Critics}% / {Audience}%", 
                    movieHit.Title, movieHit.ReleaseYear, movieHit.Scores?.CriticsScore, movieHit.Scores?.AudienceScore);
            }

            return movieHit;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching Rotten Tomatoes for {Title}", title);
            return null;
        }
    }
}
