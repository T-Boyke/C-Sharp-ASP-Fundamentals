using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tvdb.Sdk;

namespace _10_Filmdatenbank.Application.Services;

/// <summary>
/// Implementierung des ITvdbService zur Interaktion mit der TheTVDB v4 API.
/// </summary>
public class TvdbService : ITvdbService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;
    private readonly ILogger<TvdbService> _logger;
    private readonly SdkClientSettings _settings;
    private string? _token;
    private DateTime _tokenExpiration = DateTime.MinValue;

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="TvdbService"/> Klasse.
    /// </summary>
    /// <param name="configuration">Die Anwendungskonfiguration.</param>
    /// <param name="httpClient">Der HttpClient.</param>
    /// <param name="logger">Der Logger.</param>
    public TvdbService(IConfiguration configuration, HttpClient httpClient, ILogger<TvdbService> logger)
    {
        _apiKey = configuration["TVDB:ApiKey"] ?? "96f69038-0c36-4f38-ab09-8c6499575987";
        _httpClient = httpClient;
        _logger = logger;
        _settings = new SdkClientSettings { BaseUrl = "https://api4.thetvdb.com/v4/" };
        _logger.LogInformation("TVDB: Initializing TvdbService...");
    }

    private async Task EnsureAuthenticatedAsync()
    {
        if (string.IsNullOrEmpty(_token) || DateTime.UtcNow >= _tokenExpiration)
        {
            _logger.LogInformation("TVDB: Authenticating...");
            var api = new LoginClient(_settings, _httpClient);
            var response = await api.LoginAsync(new Body { Apikey = _apiKey });
            
            if (response != null && response.Status == "success" && response.Data?.Token != null)
            {
                _token = response.Data.Token;
                _tokenExpiration = DateTime.UtcNow.AddDays(28); 
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                _logger.LogInformation("TVDB: Authentication successful. Token expires at {Expiration}", _tokenExpiration);
            }
            else
            {
                _logger.LogError("TVDB: Authentication failed. Status: {Status}", response?.Status);
                throw new Exception("Fehler bei der Authentifizierung mit TheTVDB API.");
            }
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SearchResult>> SearchMoviesAsync(string query)
    {
        _logger.LogInformation("TVDB: SearchMoviesAsync called with query: {Query}", query);
        await EnsureAuthenticatedAsync();
        var api = new SearchClient(_settings, _httpClient);
        var response = await api.GetSearchResultsAsync(query, type: "movie");
        _logger.LogInformation("TVDB: SearchMoviesAsync returned {Count} results", response?.Data?.Count ?? 0);
        return response?.Data ?? Enumerable.Empty<SearchResult>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<SearchByRemoteIdResult>> GetByRemoteIdAsync(string remoteId)
    {
        _logger.LogInformation("TVDB: GetByRemoteIdAsync called with remoteId: {RemoteId}", remoteId);
        await EnsureAuthenticatedAsync();
        var api = new SearchClient(_settings, _httpClient);
        var response = await api.GetSearchResultsByRemoteIdAsync(remoteId);
        _logger.LogInformation("TVDB: GetByRemoteIdAsync returned {Count} results", response?.Data?.Count ?? 0);
        return response?.Data ?? Enumerable.Empty<SearchByRemoteIdResult>();
    }

    /// <inheritdoc />
    public async Task<MovieBaseRecord> GetMovieDetailsAsync(int tvdbId)
    {
        _logger.LogInformation("TVDB: GetMovieDetailsAsync called with tvdbId: {TvdbId}", tvdbId);
        await EnsureAuthenticatedAsync();
        var api = new MoviesClient(_settings, _httpClient);
        var response = await api.GetMovieBaseAsync((double)tvdbId);
        _logger.LogInformation("TVDB: GetMovieDetailsAsync returned movie: {Name}", response?.Data?.Name);
        return response?.Data ?? throw new Exception($"Film mit TVDB ID {tvdbId} nicht gefunden.");
    }

    /// <inheritdoc />
    public async Task<MovieExtendedRecord> GetMovieExtendedDetailsAsync(int tvdbId)
    {
        _logger.LogInformation("TVDB: GetMovieExtendedDetailsAsync called with tvdbId: {TvdbId}", tvdbId);
        await EnsureAuthenticatedAsync();
        var api = new MoviesClient(_settings, _httpClient);
        var response = await api.GetMovieExtendedAsync((double)tvdbId);
        _logger.LogInformation("TVDB: GetMovieExtendedDetailsAsync returned movie: {Name}", response?.Data?.Name);
        return response?.Data ?? throw new Exception($"Erweiterte Details für Film mit TVDB ID {tvdbId} nicht gefunden.");
    }
}
