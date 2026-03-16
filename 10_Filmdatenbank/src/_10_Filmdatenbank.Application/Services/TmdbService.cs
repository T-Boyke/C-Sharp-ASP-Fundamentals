using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;

using Microsoft.Extensions.Logging;

namespace _10_Filmdatenbank.Application.Services;

public class TmdbService : ITmdbService
{
    private readonly TMDbClient _client;
    private readonly ILogger<TmdbService> _logger;

    public TmdbService(IConfiguration configuration, ILogger<TmdbService> logger)
    {
        _logger = logger;
        _logger.LogInformation("Initializing TmdbService");
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
        _logger.LogInformation("TMDB SearchMoviesAsync called with query: {Query}, language: {Language}", query, language);
        var results = await _client.SearchMovieAsync(query, language: language);
        _logger.LogInformation("TMDB SearchMoviesAsync returned {Count} results", results.Results.Count);
        return results.Results;
    }

    public async Task<Movie> GetMovieDetailsAsync(int tmdbId, string language = "de-DE")
    {
        _logger.LogInformation("TMDB GetMovieDetailsAsync called with tmdbId: {TmdbId}, language: {Language}", tmdbId, language);
        var movie = await _client.GetMovieAsync(tmdbId, language: language, extraMethods: 
            MovieMethods.Credits | 
            MovieMethods.ExternalIds | 
            MovieMethods.Keywords | 
            MovieMethods.Videos | 
            MovieMethods.AlternativeTitles | 
            MovieMethods.ReleaseDates |
            MovieMethods.WatchProviders |
            MovieMethods.Images);
        _logger.LogInformation("TMDB GetMovieDetailsAsync returned movie: {Title}", movie?.Title);
        return movie;
    }

    public async Task<IEnumerable<SearchPerson>> SearchPersonsAsync(string query, string language = "de-DE")
    {
        _logger.LogInformation("TMDB SearchPersonsAsync called with query: {Query}, language: {Language}", query, language);
        var results = await _client.SearchPersonAsync(query, language: language);
        _logger.LogInformation("TMDB SearchPersonsAsync returned {Count} results", results.Results.Count);
        return results.Results;
    }

    public async Task<Person> GetPersonDetailsAsync(int tmdbId, string language = "de-DE")
    {
        _logger.LogInformation("TMDB GetPersonDetailsAsync called with tmdbId: {TmdbId}, language: {Language}", tmdbId, language);
        var person = await _client.GetPersonAsync(tmdbId, language: language, extraMethods: 
            PersonMethods.CombinedCredits | 
            PersonMethods.ExternalIds | 
            PersonMethods.MovieCredits |
            PersonMethods.Images |
            PersonMethods.Changes);
        _logger.LogInformation("TMDB GetPersonDetailsAsync returned person: {Name}", person?.Name);
        return person;
    }

    public async Task<IEnumerable<SearchCollection>> SearchCollectionsAsync(string query, string language = "de-DE")
    {
        _logger.LogInformation("TMDB SearchCollectionsAsync called with query: {Query}, language: {Language}", query, language);
        var results = await _client.SearchCollectionAsync(query, language: language);
        _logger.LogInformation("TMDB SearchCollectionsAsync returned {Count} results", results.Results.Count);
        return results.Results;
    }

    public async Task<Collection> GetCollectionDetailsAsync(int tmdbId, string language = "de-DE")
    {
        _logger.LogInformation("TMDB GetCollectionDetailsAsync called with tmdbId: {TmdbId}, language: {Language}", tmdbId, language);
        var collection = await _client.GetCollectionAsync(tmdbId, language: language, includeImageLanguages: null, extraMethods: CollectionMethods.Images);
        _logger.LogInformation("TMDB GetCollectionDetailsAsync returned collection: {Name}", collection?.Name);
        return collection;
    }

    public async Task<IEnumerable<SearchCompany>> SearchCompaniesAsync(string query, string language = "de-DE")
    {
        _logger.LogInformation("TMDB SearchCompaniesAsync called with query: {Query}, language: {Language}", query, language);
        var results = await _client.SearchCompanyAsync(query); 
        _logger.LogInformation("TMDB SearchCompaniesAsync returned {Count} results", results.Results.Count);
        return results.Results;
    }

    public async Task<TMDbLib.Objects.Companies.Company> GetCompanyDetailsAsync(int tmdbId)
    {
        _logger.LogInformation("TMDB GetCompanyDetailsAsync called with tmdbId: {TmdbId}", tmdbId);
        var company = await _client.GetCompanyAsync(tmdbId);
        _logger.LogInformation("TMDB GetCompanyDetailsAsync returned company: {Name}", company?.Name);
        return company;
    }
}
