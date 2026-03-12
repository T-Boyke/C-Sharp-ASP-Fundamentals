using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert ein Film-Genre von TMDB.
/// </summary>
public class Genre
{
    /// <summary>
    /// Die eindeutige Kennung des Genres in der lokalen Datenbank.
    /// </summary>
    public int GenreID { get; set; }

    /// <summary>
    /// Die ID des Genres auf TMDB.
    /// </summary>
    [Required]
    public int TmdbId { get; set; }

    /// <summary>
    /// Der Name des Genres (z.B. Action, Drama).
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Die Filme, die diesem Genre zugeordnet sind.
    /// </summary>
    public ICollection<Film> Films { get; set; } = [];
}
