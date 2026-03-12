using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert eine Filmkollektion (Filmreihe) von TMDB.
/// </summary>
public class Collection
{
    /// <summary>
    /// Die eindeutige Kennung der Kollektion in der lokalen Datenbank.
    /// </summary>
    public int CollectionID { get; set; }

    /// <summary>
    /// Die ID der Kollektion auf TMDB.
    /// </summary>
    [Required]
    public int TmdbId { get; set; }

    /// <summary>
    /// Der Name der Kollektion.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Eine kurze Übersicht/Beschreibung der Kollektion.
    /// </summary>
    public string? Overview { get; set; }

    /// <summary>
    /// Die URL zum Poster-Bild der Kollektion.
    /// </summary>
    public string? PosterUrl { get; set; }

    /// <summary>
    /// Die URL zum Hintergrundbild (Backdrop) der Kollektion.
    /// </summary>
    public string? BackdropUrl { get; set; }

    /// <summary>
    /// Die Filme, die zu dieser Kollektion gehören.
    /// </summary>
    public ICollection<Film> Films { get; set; } = [];
}
