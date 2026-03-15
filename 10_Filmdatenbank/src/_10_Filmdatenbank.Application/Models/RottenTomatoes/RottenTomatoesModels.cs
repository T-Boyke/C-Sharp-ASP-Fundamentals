using System.Text.Json.Serialization;

namespace _10_Filmdatenbank.Application.Models.RottenTomatoes;

/// <summary>
/// Root object for Algolia search request.
/// </summary>
public class AlgoliaSearchRequest
{
    [JsonPropertyName("requests")]
    public List<AlgoliaRequestItem> Requests { get; set; } = new();
}

/// <summary>
/// Individual request item in Algolia batch.
/// </summary>
public class AlgoliaRequestItem
{
    [JsonPropertyName("indexName")]
    public string IndexName { get; set; } = "content_rt";

    [JsonPropertyName("params")]
    public string Params { get; set; } = "filters=isEmsSearchable%20%3D%201&hitsPerPage=5";

    [JsonPropertyName("query")]
    public string Query { get; set; } = string.Empty;
}

/// <summary>
/// Root object for Algolia search response.
/// </summary>
public class AlgoliaSearchResponse
{
    [JsonPropertyName("results")]
    public List<AlgoliaResult> Results { get; set; } = new();
}

/// <summary>
/// Individual result in Algolia response.
/// </summary>
public class AlgoliaResult
{
    [JsonPropertyName("hits")]
    public List<RottenTomatoesHit> Hits { get; set; } = new();
}

/// <summary>
/// A movie/show hit from Rotten Tomatoes.
/// </summary>
public class RottenTomatoesHit
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("releaseYear")]
    public int? ReleaseYear { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("vanity")]
    public string Vanity { get; set; } = string.Empty;

    [JsonPropertyName("rottenTomatoes")]
    public RottenTomatoesScores? Scores { get; set; }
}

/// <summary>
/// Scores for a movie on Rotten Tomatoes.
/// </summary>
public class RottenTomatoesScores
{
    [JsonPropertyName("criticsScore")]
    public int? CriticsScore { get; set; }

    [JsonPropertyName("audienceScore")]
    public int? AudienceScore { get; set; }

    [JsonPropertyName("certifiedFresh")]
    public bool CertifiedFresh { get; set; }
}
