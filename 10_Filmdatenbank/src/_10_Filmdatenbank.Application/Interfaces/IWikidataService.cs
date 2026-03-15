using System.Threading.Tasks;

namespace _10_Filmdatenbank.Application.Interfaces;

/// <summary>
/// Result of an enriched person from Wikidata.
/// </summary>
public class WikidataPersonResult
{
    public string? BirthPlace { get; set; }
    public string? ZodiacSign { get; set; }
    public string? InstagramId { get; set; }
    public string? TwitterId { get; set; }
    public string? FacebookId { get; set; }
    public string? Description { get; set; }
    public List<WikidataAward> Awards { get; set; } = new();
}

public class WikidataAward
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Year { get; set; }
    public bool IsWin { get; set; } // Wikidata 'award received' usually implies a win
}

/// <summary>
/// Result of an enriched production company from Wikidata.
/// </summary>
public class WikidataCompanyResult
{
    public int? FoundedYear { get; set; }
    public string? Headquarters { get; set; }
    public string? EmployeeCount { get; set; }
    public string? ParentCompany { get; set; }
}

public interface IWikidataService
{
    /// <summary>
    /// Fetches enriched data for a person using their Wikidata ID or IMDb ID.
    /// </summary>
    Task<WikidataPersonResult?> GetPersonDetailsAsync(string? wikidataId, string? imdbId = null);

    /// <summary>
    /// Fetches enriched data for a company using their Wikidata ID or TMDB Company ID.
    /// </summary>
    Task<WikidataCompanyResult?> GetCompanyDetailsAsync(string? wikidataId, int? tmdbId = null);
}
