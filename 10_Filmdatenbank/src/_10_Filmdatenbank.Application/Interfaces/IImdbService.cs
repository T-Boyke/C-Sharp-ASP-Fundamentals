using System.Threading.Tasks;

namespace _10_Filmdatenbank.Application.Interfaces;

/// <summary>
/// Metadata fetched from IMDb.
/// </summary>
public class ImdbMetadata
{
    public double? Rating { get; set; }
    public int? Metascore { get; set; }
}

/// <summary>
/// Interface for fetching movie data from IMDb.
/// </summary>
public interface IImdbService
{
    /// <summary>
    /// Fetches the metadata (Rating, Metascore) for a movie by its IMDb ID.
    /// </summary>
    /// <param name="imdbId">The IMDb ID (e.g., tt0114709).</param>
    /// <returns>The metadata or null if not found.</returns>
    Task<ImdbMetadata?> GetMetadataAsync(string imdbId);
}
