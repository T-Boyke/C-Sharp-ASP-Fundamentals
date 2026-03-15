using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace _10_Filmdatenbank.Application.Services;

/// <summary>
/// Implementation of IImdbService that fetches ratings by parsing IMDb's public LD+JSON metadata.
/// </summary>
public class ImdbService : IImdbService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ImdbService> _logger;

    public ImdbService(HttpClient httpClient, ILogger<ImdbService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        
        // Add User-Agent to avoid being blocked
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36");
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
    }

    /// <inheritdoc />
    public async Task<ImdbMetadata?> GetMetadataAsync(string imdbId)
    {
        if (string.IsNullOrEmpty(imdbId)) return null;

        try
        {
            var url = $"https://www.imdb.com/title/{imdbId}/";
            var html = await _httpClient.GetStringAsync(url);

            var metadata = new ImdbMetadata();

            // Part 1: LD+JSON (Fallback for Rating)
            var ldMatch = Regex.Match(html, @"<script type=""application/ld\+json"">(.*?)</script>", RegexOptions.Singleline);
            if (ldMatch.Success)
            {
                try {
                    using var ldDoc = JsonDocument.Parse(ldMatch.Groups[1].Value);
                    if (ldDoc.RootElement.TryGetProperty("aggregateRating", out var agg) && agg.TryGetProperty("ratingValue", out var val))
                    {
                        if (val.ValueKind == JsonValueKind.Number) metadata.Rating = val.GetDouble();
                        else if (val.ValueKind == JsonValueKind.String && double.TryParse(val.GetString(), out var p)) metadata.Rating = p;
                    }
                } catch { /* ignore parse error */ }
            }

            // Part 2: __NEXT_DATA__ (Primary for Metascore and robust Rating)
            var nextMatch = Regex.Match(html, @"<script id=""__NEXT_DATA__"" type=""application/json"">(.*?)</script>", RegexOptions.Singleline);
            if (nextMatch.Success)
            {
                try {
                    using var nextDoc = JsonDocument.Parse(nextMatch.Groups[1].Value);
                    var root = nextDoc.RootElement;
                    
                    // Access props -> pageProps -> aboveTheFoldData
                    if (root.TryGetProperty("props", out var props) && 
                        props.TryGetProperty("pageProps", out var pageProps) &&
                        pageProps.TryGetProperty("aboveTheFoldData", out var data))
                    {
                        // 1. Extract Metascore
                        if (data.TryGetProperty("metacritic", out var mc) && 
                            mc.TryGetProperty("metascore", out var ms) &&
                            ms.TryGetProperty("score", out var score) &&
                            score.ValueKind == JsonValueKind.Number)
                        {
                            metadata.Metascore = score.GetInt32();
                        }

                        // 2. Extract Rating (often more up-to-date here)
                        if (metadata.Rating == null && 
                            data.TryGetProperty("ratingsSummary", out var rs) &&
                            rs.TryGetProperty("aggregateRating", out var ar) &&
                            ar.ValueKind == JsonValueKind.Number)
                        {
                            metadata.Rating = ar.GetDouble();
                        }
                    }
                } catch { /* ignore parse error */ }
            }

            if (metadata.Rating == null && metadata.Metascore == null)
            {
                _logger.LogWarning("IMDb: No metadata found for {ImdbId}", imdbId);
                return null;
            }

            return metadata;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "IMDb: Error fetching metadata for {ImdbId}", imdbId);
            return null;
        }
    }
}
