using System.Collections.Generic;
using System.Threading.Tasks;
using Tvdb.Sdk.Model;

namespace _10_Filmdatenbank.Application.Interfaces;

/// <summary>
/// Definiert den Dienst für die Interaktion mit der TheTVDB v4 API.
/// </summary>
public interface ITvdbService
{
    /// <summary>
    /// Sucht nach Filmen auf TheTVDB.
    /// </summary>
    /// <param name="query">Der Suchbegriff.</param>
    /// <returns>Eine Liste von Suchergebnissen.</returns>
    Task<IEnumerable<SearchResult>> SearchMoviesAsync(string query);

    /// <summary>
    /// Ruft detaillierte Informationen zu einem Film ab.
    /// </summary>
    /// <param name="tvdbId">Die TVDB-ID des Films.</param>
    /// <returns>Die Filmdetails.</returns>
    Task<MovieBaseRecord> GetMovieDetailsAsync(int tvdbId);

    /// <summary>
    /// Ruft erweiterte Informationen zu einem Film ab.
    /// </summary>
    /// <param name="tvdbId">Die TVDB-ID des Films.</param>
    /// <returns>Das erweiterte Filmmodell.</returns>
    Task<MovieExtendedRecord> GetMovieExtendedDetailsAsync(int tvdbId);
}
