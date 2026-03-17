using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using _10_Filmdatenbank.Application.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace _10_Filmdatenbank.Application.Services;

/// <summary>
/// Implementation of IMetacriticService using deep scraping of metacritic.com.
/// </summary>
public class MetacriticService : IMetacriticService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MetacriticService> _logger;

    public MetacriticService(HttpClient httpClient, ILogger<MetacriticService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        // Use a modern user agent to avoid being blocked
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
    }

    /// <inheritdoc />
    public async Task<MetacriticData?> GetDeepMetadataAsync(string title, int? year = null)
    {
        if (string.IsNullOrEmpty(title)) return null;

        try
        {
            // 1. First, search for the movie to get the slug
            // Sample: https://www.metacritic.com/search/the%20matrix/
            var searchUrl = $"https://www.metacritic.com/search/{Uri.EscapeDataString(title)}/?category=13"; // category 13 is movies
            var searchHtml = await _httpClient.GetStringAsync(searchUrl);
            var searchDoc = new HtmlDocument();
            searchDoc.LoadHtml(searchHtml);

            // Access __NEXT_DATA__ for the most reliable results in modern Metacritic
            var nextMatch = Regex.Match(searchHtml, @"<script id=""__NEXT_DATA__"" type=""application/json"">(.*?)</script>", RegexOptions.Singleline);
            string? slug = null;

            if (nextMatch.Success)
            {
                try
                {
                    using var nextDoc = JsonDocument.Parse(nextMatch.Groups[1].Value);
                    var searchResults = nextDoc.RootElement
                        .GetProperty("props")
                        .GetProperty("pageProps")
                        .GetProperty("searchResults")
                        .GetProperty("results");

                    foreach (var result in searchResults.EnumerateArray())
                    {
                        var resultTitle = result.GetProperty("title").GetString();
                        var releaseDate = result.GetProperty("releaseDate").GetString();
                        var type = result.GetProperty("type").GetString();

                        if (type == "movie" && 
                            (resultTitle?.Equals(title, StringComparison.OrdinalIgnoreCase) == true))
                        {
                            // Check year if provided
                            if (year.HasValue && !string.IsNullOrEmpty(releaseDate) && !releaseDate.Contains(year.Value.ToString()))
                                continue;

                            slug = result.GetProperty("slug").GetString();
                            if (!string.IsNullOrEmpty(slug)) break;
                        }
                    }
                }
                catch { /* fallback to manual scraping or ignore error */ }
            }

            // Fallback: If slug is still null, try manual scraping or a simple slugifier
            if (string.IsNullOrEmpty(slug))
            {
                slug = title.ToLower().Replace(" ", "-").Replace(":", "").Replace("'", "");
                _logger.LogWarning("Metacritic: Suggesting slug '{Slug}' as fallback for '{Title}'", slug, title);
            }

            // 2. Fetch the movie page
            var movieUrl = $"https://www.metacritic.com/movie/{slug}/";
            var movieHtml = await _httpClient.GetStringAsync(movieUrl);
            var movieDoc = new HtmlDocument();
            movieDoc.LoadHtml(movieHtml);

            var data = new MetacriticData { MetacriticUrl = movieUrl };

            // 3. Extract scores from __NEXT_DATA__ if available (modern Metacritic)
            var movieNextMatch = Regex.Match(movieHtml, @"<script id=""__NEXT_DATA__"" type=""application/json"">(.*?)</script>", RegexOptions.Singleline);
            if (movieNextMatch.Success)
            {
                try
                {
                    using var nextDoc = JsonDocument.Parse(movieNextMatch.Groups[1].Value);
                    var props = nextDoc.RootElement.GetProperty("props").GetProperty("pageProps").GetProperty("components");
                    
                    // Look for scores component
                    // Depending on the structure this might vary, but in current Metacritic it's often in a hero or header component.
                    foreach (var component in props.EnumerateArray())
                    {
                        if (component.TryGetProperty("data", out var compData))
                        {
                            if (compData.TryGetProperty("score", out var score) && score.ValueKind == JsonValueKind.Number)
                                data.Metascore = score.GetInt32();

                            if (compData.TryGetProperty("userScore", out var us) && us.ValueKind == JsonValueKind.Number)
                                data.UserScore = us.GetDouble();
                        }
                    }
                }
                catch { /* ignore */ }
            }

            // 4. Extraction via CSS (Fallback or complementary)
            if (data.Metascore == null)
            {
                var metascoreNode = movieDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'c-productScoreInfo_scoreNumber')]//span");
                if (metascoreNode != null && int.TryParse(metascoreNode.InnerText.Trim(), out var ms))
                    data.Metascore = ms;
            }

            // 5. Critic Reviews (Deep Scraping part)
            // Look for reviews in HTML cards
            var reviewCards = movieDoc.DocumentNode.SelectNodes("//div[contains(@class, 'c-siteReview')]");
            if (reviewCards != null)
            {
                foreach (var card in reviewCards)
                {
                    if (data.Reviews.Count >= 5) break;

                    var snippetNode = card.SelectSingleNode(".//div[contains(@class, 'c-siteReview_quote')]");
                    var publicationNode = card.SelectSingleNode(".//a[contains(@class, 'c-siteReview_publicationName')]");
                    var scoreNode = card.SelectSingleNode(".//div[contains(@class, 'c-siteReviewScore')]//span");
                    var authorNode = card.SelectSingleNode(".//div[contains(@class, 'c-siteReview_author')]");

                    if (snippetNode != null && publicationNode != null)
                    {
                        var review = new MetacriticReviewSnippet
                        {
                            Snippet = snippetNode.InnerText.Trim(),
                            Publication = publicationNode.InnerText.Trim(),
                            Author = authorNode?.InnerText.Trim()
                        };

                        if (scoreNode != null && int.TryParse(scoreNode.InnerText.Trim(), out var rs))
                            review.Score = rs;

                        data.Reviews.Add(review);
                    }
                }
            }

            _logger.LogInformation("Metacritic: Deep Scraping Success for '{Title}': Metascore: {Score}, User: {User}, Reviews: {Count}", 
                title, data.Metascore, data.UserScore, data.Reviews.Count);

            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Metacritic: Error deep scraping for '{Title}'", title);
            return null;
        }
    }
}
