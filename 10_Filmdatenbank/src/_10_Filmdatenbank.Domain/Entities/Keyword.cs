using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert ein Schlagwort (Keyword) von TMDB.
/// </summary>
public class Keyword
{
    /// <summary>
    /// Die eindeutige Kennung des Schlagworts in der lokalen Datenbank.
    /// </summary>
    public int KeywordID { get; set; }

    /// <summary>
    /// Die ID des Schlagworts auf TMDB.
    /// </summary>
    [Required]
    public int TmdbId { get; set; }

    /// <summary>
    /// Der Inhalt des Schlagworts (z.B. "time travel", "superhero").
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Die Filme, die mit diesem Schlagwort markiert sind.
    /// </summary>
    public ICollection<Film> Films { get; set; } = [];
}
