using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading.Tasks;
using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;
using Newtonsoft.Json;

namespace _10_Filmdatenbank.Application.Services;

/// <summary>
/// Hoch-resilienter Service zur Interaktion mit der TMDB API.
/// Nutzt einen SocketsHttpHandler mit deaktiviertem HTTP/2 und erzwungenem IPv4, 
/// um SSL-Handshake-Abbrüche (10054) zu verhindern.
/// </summary>
public class TmdbService : ITmdbService
{
    private readonly TMDbClient _client;
    private readonly ILogger<TmdbService> _logger;
    private readonly string _apiKey;
    private readonly string? _accessToken;
    private static readonly HttpClient _httpClient;
    private static readonly System.Text.Json.JsonSerializerOptions _jsonOptions;

    static TmdbService()
    {
        // Wir nutzen den SocketsHttpHandler für maximale Kontrolle über den Handshake
        var handler = new SocketsHttpHandler
        {
            // Erzwungene TLS-Protokolle
            SslOptions = new System.Net.Security.SslClientAuthenticationOptions
            {
                EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
                RemoteCertificateValidationCallback = (sender, cert, chain, errors) => true
            },
            // WICHTIG: Manche ISP/Router (FritzBox) haben Probleme mit TMDB via IPv6.
            // Wir erzwingen hier IPv4 (InterNetwork) für die Verbindung.
            ConnectCallback = async (context, cancellationToken) =>
            {
                var host = context.DnsEndPoint.Host;
                // Wir holen gezielt nur IPv4 Adressen
                var addresses = await Dns.GetHostAddressesAsync(host, AddressFamily.InterNetwork, cancellationToken);

                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.NoDelay = true;

                try
                {
                    await socket.ConnectAsync(addresses, context.DnsEndPoint.Port, cancellationToken);
                    return new NetworkStream(socket, true);
                }
                catch
                {
                    socket.Dispose();
                    throw;
                }
            },
            // WICHTIG: Manche Firewalls/Proxies brechen bei HTTP/2 Handshakes von .NET ab.
            // Wir erzwingen hier HTTP/1.1 für maximale Kompatibilität.
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        _httpClient = new HttpClient(handler);
        _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "TmdbResilientClient/1.0");
        _httpClient.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact;
        _httpClient.DefaultRequestVersion = HttpVersion.Version11;
        _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");

        _jsonOptions = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true
        };
    }

    public TmdbService(IConfiguration configuration, ILogger<TmdbService> logger)
    {
        _logger = logger;
        _apiKey = configuration["TMDB:ApiKey"] ?? "d32c6254aebfa68d0c01e5995711ffc1";
        _accessToken = configuration["TMDB:AccessToken"];

        _logger.LogInformation("Initializing Resilient TmdbService (IPv4 & HTTP/1.1 enforced)");

        if (!string.IsNullOrEmpty(_accessToken) && _httpClient.DefaultRequestHeaders.Authorization == null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
        }

        // Initialisierung des Standard-Clients für Fallbacks
        _client = new TMDbClient(_apiKey);
    }

    public async Task<IEnumerable<SearchMovie>> SearchMoviesAsync(string query, string language = "de-DE")
    {
        _logger.LogInformation("TMDB SearchMoviesAsync: {Query}", query);
        try
        {
            // Manueller Pfad über HttpClient (Erzwingt IPv4 und HTTP/1.1)
            var url = $"search/movie?query={Uri.EscapeDataString(query)}&language={language}";
            var json = await _httpClient.GetStringAsync(url);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<TmdbSearchResponse<SearchMovie>>(json);
            return response?.Results ?? Enumerable.Empty<SearchMovie>();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Manual Resilient Search failed ({Message}). Attempting library fallback.", ex.Message);
            try
            {
                // Da _client nun unseren _httpClient nutzt, sollte auch dieser IPv4 erzwingen
                var fallback = await _client.SearchMovieAsync(query, language: language);
                return fallback?.Results ?? Enumerable.Empty<SearchMovie>();
            }
            catch (Exception ex2)
            {
                _logger.LogError(ex2, "CRITICAL: All connection attempts to TMDB failed (IPv4/10054).");
                throw;
            }
        }
    }

    public async Task<Movie?> GetMovieDetailsAsync(int tmdbId, string language = "de-DE")
    {
        try
        {
            var url = $"movie/{tmdbId}?language={language}&append_to_response=credits,external_ids,keywords,videos,alternative_titles,release_dates,watch/providers,images";
            var json = await _httpClient.GetStringAsync(url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Movie>(json);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Manual Details fetch failed, trying library fallback.");
            return await _client.GetMovieAsync(tmdbId, language: language, extraMethods:
                MovieMethods.Credits | MovieMethods.ExternalIds | MovieMethods.Keywords |
                MovieMethods.Videos | MovieMethods.AlternativeTitles | MovieMethods.ReleaseDates |
                MovieMethods.WatchProviders | MovieMethods.Images);
        }
    }

    private class TmdbSearchResponse<T> { public List<T>? Results { get; set; } }

    public async Task<IEnumerable<SearchPerson>> SearchPersonsAsync(string query, string language = "de-DE")
    {
        try
        {
            var url = $"search/person?query={Uri.EscapeDataString(query)}&language={language}";
            var json = await _httpClient.GetStringAsync(url);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<TmdbSearchResponse<SearchPerson>>(json);
            return response?.Results ?? Enumerable.Empty<SearchPerson>();
        }
        catch
        {
            return (await _client.SearchPersonAsync(query, language: language))?.Results ?? Enumerable.Empty<SearchPerson>();
        }
    }

    public async Task<Person?> GetPersonDetailsAsync(int tmdbId, string language = "de-DE")
    {
        try
        {
            var url = $"person/{tmdbId}?language={language}&append_to_response=combined_credits,external_ids";
            var json = await _httpClient.GetStringAsync(url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Person>(json);
        }
        catch
        {
            return await _client.GetPersonAsync(tmdbId, language: language, extraMethods: PersonMethods.CombinedCredits | PersonMethods.ExternalIds);
        }
    }

    public async Task<IEnumerable<SearchCollection>> SearchCollectionsAsync(string query, string language = "de-DE")
    {
        try
        {
            var url = $"search/collection?query={Uri.EscapeDataString(query)}&language={language}";
            var json = await _httpClient.GetStringAsync(url);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<TmdbSearchResponse<SearchCollection>>(json);
            return response?.Results ?? Enumerable.Empty<SearchCollection>();
        }
        catch
        {
            return (await _client.SearchCollectionAsync(query, language: language))?.Results ?? Enumerable.Empty<SearchCollection>();
        }
    }

    public async Task<Collection?> GetCollectionDetailsAsync(int tmdbId, string language = "de-DE")
    {
        try
        {
            var url = $"collection/{tmdbId}?language={language}";
            var json = await _httpClient.GetStringAsync(url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Collection>(json);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Manual Collection fetch failed, trying library fallback (Warning: May fail with 10054).");
            // Wir lassen den Fallback drin, warnen aber, dass er instabil sein könnte.
            try
            {
                return await _client.GetCollectionAsync(tmdbId, language: language, includeImageLanguages: null, extraMethods: CollectionMethods.Undefined);
            }
            catch (Exception ex2)
            {
                _logger.LogError(ex2, "CRITICAL: Collection fetch failed on all paths.");
                return null;
            }
        }
    }

    public async Task<IEnumerable<SearchCompany>> SearchCompaniesAsync(string query, string language = "de-DE")
    {
        try
        {
            var url = $"search/company?query={Uri.EscapeDataString(query)}";
            var json = await _httpClient.GetStringAsync(url);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<TmdbSearchResponse<SearchCompany>>(json);
            return response?.Results ?? Enumerable.Empty<SearchCompany>();
        }
        catch
        {
            return (await _client.SearchCompanyAsync(query))?.Results ?? Enumerable.Empty<SearchCompany>();
        }
    }

    public async Task<TMDbLib.Objects.Companies.Company?> GetCompanyDetailsAsync(int tmdbId)
    {
        try
        {
            var url = $"company/{tmdbId}";
            var json = await _httpClient.GetStringAsync(url);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TMDbLib.Objects.Companies.Company>(json);
        }
        catch
        {
            return await _client.GetCompanyAsync(tmdbId);
        }
    }
}
