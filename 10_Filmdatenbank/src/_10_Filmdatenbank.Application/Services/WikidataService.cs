using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using _10_Filmdatenbank.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace _10_Filmdatenbank.Application.Services;

public class WikidataService : IWikidataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WikidataService> _logger;

    public WikidataService(HttpClient httpClient, ILogger<WikidataService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        
        // Wikidata requires a descriptive User-Agent
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CouchDB-App/1.0 (contact: admin@couchdb.local) .NET/8.0");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/sparql-results+json");
    }

    public async Task<WikidataPersonResult?> GetPersonDetailsAsync(string? wikidataId, string? imdbId = null)
    {
        var targetId = wikidataId;
        
        // If we don't have a Wikidata ID but have an IMDb ID, we can find the Wikidata entity first
        if (string.IsNullOrEmpty(targetId) && !string.IsNullOrEmpty(imdbId))
        {
            targetId = await FindWikidataIdByImdbId(imdbId);
        }

        if (string.IsNullOrEmpty(targetId)) return null;

        var sparql = $@"
        SELECT ?birthPlaceLabel ?zodiacLabel ?insta ?twitter ?fb ?desc WHERE {{
          BIND(wd:{targetId} AS ?person)
          OPTIONAL {{ ?person wdt:P19 ?birthPlace. }}
          OPTIONAL {{ ?person wdt:P2571 ?zodiac. }}
          OPTIONAL {{ ?person wdt:P2003 ?insta. }}
          OPTIONAL {{ ?person wdt:P2002 ?twitter. }}
          OPTIONAL {{ ?person wdt:P2013 ?fb. }}
          OPTIONAL {{ ?person schema:description ?desc. FILTER(LANG(?desc) = ""de"") }}
          SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""de,en"". }}
        }} LIMIT 1";

        try
        {
            var results = await ExecuteSparqlAsync(sparql);
            WikidataPersonResult? personResult = null;

            if (results != null && results.Length > 0)
            {
                var r = results[0];
                personResult = new WikidataPersonResult
                {
                    BirthPlace = GetVal(r, "birthPlaceLabel"),
                    ZodiacSign = GetVal(r, "zodiacLabel"),
                    InstagramId = GetVal(r, "insta"),
                    TwitterId = GetVal(r, "twitter"),
                    FacebookId = GetVal(r, "fb"),
                    Description = GetVal(r, "desc")
                };
            }

            if (personResult != null)
            {
                personResult.Awards = await GetPersonAwardsAsync(targetId);
            }

            return personResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Wikidata: Error fetching person details for {Id}", targetId);
            return null;
        }
    }

    private async Task<List<WikidataAward>> GetPersonAwardsAsync(string wikidataId)
    {
        var awards = new List<WikidataAward>();
        var sparql = $@"
        SELECT ?awardLabel ?date ?workLabel WHERE {{
          BIND(wd:{wikidataId} AS ?person)
          ?person p:P166 ?statement.
          ?statement ps:P166 ?award.
          OPTIONAL {{ ?statement pq:P585 ?date. }}
          OPTIONAL {{ ?statement pq:P1686 ?work. }}
          SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""de,en"". }}
        }}";

        try
        {
            var results = await ExecuteSparqlAsync(sparql);
            foreach (var r in results)
            {
                var name = GetVal(r, "awardLabel");
                var work = GetVal(r, "workLabel");
                var dateStr = GetVal(r, "date");
                
                if (string.IsNullOrEmpty(name)) continue;

                var award = new WikidataAward
                {
                    Name = name,
                    Category = work ?? "Win", // If work is specified, it's 'for work', otherwise generic
                    IsWin = true
                };

                if (!string.IsNullOrEmpty(dateStr) && DateTime.TryParse(dateStr, out var d))
                {
                    award.Year = d.Year;
                }
                
                awards.Add(award);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Wikidata: Error fetching awards for {Id}", wikidataId);
        }

        return awards;
    }

    public async Task<WikidataCompanyResult?> GetCompanyDetailsAsync(string? wikidataId, int? tmdbId = null)
    {
        var targetId = wikidataId;

        if (string.IsNullOrEmpty(targetId) && tmdbId.HasValue)
        {
            targetId = await FindWikidataIdByTmdbCompanyId(tmdbId.Value);
        }

        if (string.IsNullOrEmpty(targetId)) return null;

        var sparql = $@"
        SELECT ?inception ?hqLabel ?employees ?parentLabel WHERE {{
          BIND(wd:{targetId} AS ?company)
          OPTIONAL {{ ?company wdt:P571 ?inception. }}
          OPTIONAL {{ ?company wdt:P159 ?hq. }}
          OPTIONAL {{ ?company wdt:P1128 ?employees. }}
          OPTIONAL {{ ?company wdt:P176 ?parent. }}
          SERVICE wikibase:label {{ bd:serviceParam wikibase:language ""de,en"". }}
        }} LIMIT 1";

        try
        {
            var results = await ExecuteSparqlAsync(sparql);
            if (results == null || results.Length == 0) return null;

            var r = results[0];
            var result = new WikidataCompanyResult
            {
                Headquarters = GetVal(r, "hqLabel"),
                EmployeeCount = GetVal(r, "employees"),
                ParentCompany = GetVal(r, "parentLabel")
            };

            var inceptionStr = GetVal(r, "inception");
            if (!string.IsNullOrEmpty(inceptionStr) && DateTime.TryParse(inceptionStr, out var date))
            {
                result.FoundedYear = date.Year;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Wikidata: Error fetching company details for {Id}", wikidataId);
            return null;
        }
    }

    private async Task<string?> FindWikidataIdByTmdbCompanyId(int tmdbId)
    {
        var sparql = $@"
        SELECT ?item WHERE {{
          ?item wdt:P3342 ""{tmdbId}"".
        }} LIMIT 1";

        var results = await ExecuteSparqlAsync(sparql);
        if (results != null && results.Length > 0)
        {
            var uri = GetVal(results[0], "item");
            if (!string.IsNullOrEmpty(uri))
            {
                return uri.Split('/').Last();
            }
        }
        return null;
    }

    private async Task<string?> FindWikidataIdByImdbId(string imdbId)
    {
        var sparql = $@"
        SELECT ?item WHERE {{
          ?item wdt:P345 ""{imdbId}"".
        }} LIMIT 1";

        var results = await ExecuteSparqlAsync(sparql);
        if (results != null && results.Length > 0)
        {
            var uri = GetVal(results[0], "item");
            if (!string.IsNullOrEmpty(uri))
            {
                return uri.Split('/').Last();
            }
        }
        return null;
    }

    private async Task<Dictionary<string, string>[]> ExecuteSparqlAsync(string query)
    {
        var url = "https://query.wikidata.org/sparql?query=" + Uri.EscapeDataString(query);
        var response = await _httpClient.GetStringAsync(url);
        
        using var doc = JsonDocument.Parse(response);
        var bindings = doc.RootElement
            .GetProperty("results")
            .GetProperty("bindings");

        var list = new List<Dictionary<string, string>>();
        foreach (var item in bindings.EnumerateArray())
        {
            var dict = new Dictionary<string, string>();
            foreach (var prop in item.EnumerateObject())
            {
                if (prop.Value.TryGetProperty("value", out var val))
                {
                    var s = val.GetString();
                    if (s != null) dict[prop.Name] = s;
                }
            }
            list.Add(dict);
        }
        return list.ToArray();
    }

    private string? GetVal(Dictionary<string, string> dict, string key)
    {
        return dict.TryGetValue(key, out var val) ? val : null;
    }
}
