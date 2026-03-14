using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Tvdb.Sdk;

namespace _10_Filmdatenbank.Application.Services;

/// <summary>
/// Implementierung des ITvdbService zur Interaktion mit der TheTVDB v4 API.
/// </summary>
public class TvdbService : ITvdbService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private string? _token;
    private DateTime _tokenExpiration = DateTime.MinValue;

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="TvdbService"/> Klasse.
    /// </summary>
    /// <param name="configuration">Die Anwendungskonfiguration.</param>
    /// <param name="httpClientFactory">Die Factory für HttpClients.</param>
    public TvdbService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _apiKey = configuration["TVDB:ApiKey"] ?? "96f69038-0c36-4f38-ab09-8c6499575987";
        _httpClient = httpClientFactory.CreateClient("TVDB");
    }

    private async Task EnsureAuthenticatedAsync()
    {
        if (string.IsNullOrEmpty(_token) || DateTime.UtcNow >= _tokenExpiration)
        {
            var api = new TvdbClient(_httpClient);
            var response = await api.LoginAsync(new AuthLoginPostRequest { Apikey = _apiKey });
            
            if (response?.Status == "success" && response.Data?.Token != null)
            {
                _token = response.Data.Token;
                _tokenExpiration = DateTime.UtcNow.AddDays(28); // Token ist ca. 30 Tage gültig
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            }
            else
            {
                throw new Exception("Fehler bei der Authentifizierung mit TheTVDB API.");
            }
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SearchResult>> SearchMoviesAsync(string query)
    {
        await EnsureAuthenticatedAsync();
        var api = new TvdbClient(_httpClient);
        var response = await api.GetSearchResultsAsync(query, type: "movie");
        return response?.Data ?? Enumerable.Empty<SearchResult>();
    }

    /// <inheritdoc />
    public async Task<MovieBaseRecord> GetMovieDetailsAsync(int tvdbId)
    {
        await EnsureAuthenticatedAsync();
        var api = new TvdbClient(_httpClient);
        var response = await api.GetMovieBaseAsync(tvdbId.ToString());
        return response?.Data ?? throw new Exception($"Film mit TVDB ID {tvdbId} nicht gefunden.");
    }

    /// <inheritdoc />
    public async Task<MovieExtendedRecord> GetMovieExtendedDetailsAsync(int tvdbId)
    {
        await EnsureAuthenticatedAsync();
        var api = new TvdbClient(_httpClient);
        var response = await api.GetMovieExtendedAsync(tvdbId.ToString());
        return response?.Data ?? throw new Exception($"Erweiterte Details für Film mit TVDB ID {tvdbId} nicht gefunden.");
    }
}
