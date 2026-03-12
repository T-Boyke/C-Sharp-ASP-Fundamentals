using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace _10_Filmdatenbank.Web.Utilities;

public class TmdbEnrichmentTool
{
    private readonly string _apiKey = "d32c6254aebfa68d0c01e5995711ffc1";
    private readonly string _bearerToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJkMzJjNjI1NGFlYmZhNjhkMGMwMWU1OTk1NzExZmZjMSIsIm5iZiI6MTQ2ODI1MjEwNC43MjEsInN1YiI6IjU3ODNiZmM4YzNhMzY4NDIxODAwMjk0NyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.3SGYixE54HrGWxxk9N7mzNAt_r2LE2ol2jsmyJAou3Q";
    private readonly HttpClient _httpClient;

    public TmdbEnrichmentTool()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.themoviedb.org/3/");
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
    }

    public async Task<string?> GetMovieDataAsync(string title, int? year = null)
    {
        var url = $"search/movie?query={Uri.EscapeDataString(title)}&language=de-DE";
        if (year.HasValue) url += $"&year={year.Value}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var content = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;

        if (root.GetProperty("results").GetArrayLength() == 0) return null;

        var firstResult = root.GetProperty("results")[0];
        var movieId = firstResult.GetProperty("id").GetInt32();

        return await GetMovieDetailsAsync(movieId);
    }

    private async Task<string?> GetMovieDetailsAsync(int movieId)
    {
        var url = $"movie/{movieId}?language=de-DE&append_to_response=credits";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadAsStringAsync();
    }
}
