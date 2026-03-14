using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;

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
        return await _client.GetMovieAsync(tmdbId, language: language, extraMethods: 
            MovieMethods.Credits | 
            MovieMethods.ExternalIds | 
            MovieMethods.Keywords | 
            MovieMethods.Videos | 
            MovieMethods.AlternativeTitles | 
            MovieMethods.ReleaseDates |
            MovieMethods.Images);
    }

    public async Task<IEnumerable<SearchPerson>> SearchPersonsAsync(string query, string language = "de-DE")
    {
        var results = await _client.SearchPersonAsync(query, language: language);
        return results.Results;
    }

    public async Task<Person> GetPersonDetailsAsync(int tmdbId, string language = "de-DE")
    {
        return await _client.GetPersonAsync(tmdbId, language: language, extraMethods: 
            PersonMethods.CombinedCredits | 
            PersonMethods.ExternalIds | 
            PersonMethods.MovieCredits |
            PersonMethods.Images |
            PersonMethods.Changes);
    }

    public async Task<IEnumerable<SearchCollection>> SearchCollectionsAsync(string query, string language = "de-DE")
    {
        var results = await _client.SearchCollectionAsync(query, language: language);
        return results.Results;
    }

    public async Task<Collection> GetCollectionDetailsAsync(int tmdbId, string language = "de-DE")
    {
        return await _client.GetCollectionAsync(tmdbId, language: language, includeImageLanguages: null, extraMethods: CollectionMethods.Images);
    }

    public async Task<IEnumerable<SearchCompany>> SearchCompaniesAsync(string query, string language = "de-DE")
    {
        var results = await _client.SearchCompanyAsync(query); // SearchCompany doesn't always support language in current TMDbLib
        return results.Results;
    }

    public async Task<TMDbLib.Objects.Companies.Company> GetCompanyDetailsAsync(int tmdbId)
    {
        return await _client.GetCompanyAsync(tmdbId);
    }
}
