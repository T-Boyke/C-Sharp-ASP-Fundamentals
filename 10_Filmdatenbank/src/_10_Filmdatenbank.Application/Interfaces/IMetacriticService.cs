using System.Threading.Tasks;

namespace _10_Filmdatenbank.Application.Interfaces;

public interface IMetacriticService
{
    /// <summary>
    /// Fetches deep metadata and review snippets from Metacritic.
    /// </summary>
    /// <param name="title">The movie title.</param>
    /// <param name="year">The release year.</param>
    /// <returns>Metacritic data object.</returns>
    Task<MetacriticData?> GetDeepMetadataAsync(string title, int? year = null);
}

public class MetacriticData
{
    public int? Metascore { get; set; }
    public double? UserScore { get; set; }
    public string? MetacriticUrl { get; set; }
    public List<MetacriticReviewSnippet> Reviews { get; set; } = new();
}

public class MetacriticReviewSnippet
{
    public string? Author { get; set; }
    public string? Publication { get; set; }
    public int? Score { get; set; }
    public string? Snippet { get; set; }
    public string? ReviewUrl { get; set; }
}
