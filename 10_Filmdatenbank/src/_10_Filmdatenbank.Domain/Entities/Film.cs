using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert einen Film in der Datenbank.
/// </summary>
public class Film
{
    /// <summary>
    /// Die eindeutige Kennung des Films.
    /// </summary>
    public int FilmID { get; set; }

    /// <summary>
    /// Der Titel des Films.
    /// </summary>
    public string Titel { get; set; } = string.Empty;

    /// <summary>
    /// Das Erscheinungsjahr des Films.
    /// </summary>
    public int Erscheinungsjahr { get; set; }

    /// <summary>
    /// Die Spieldauer des Films in Minuten.
    /// </summary>
    public int Spieldauer { get; set; }

    /// <summary>
    /// Der Preis des Films in Euro.
    /// </summary>
    public decimal Preis { get; set; }

    /// <summary>
    /// Die detaillierte Handlung des Films.
    /// </summary>
    public string? Handlung { get; set; }

    /// <summary>
    /// Die URL zum Poster-Bild.
    /// </summary>
    public string? PosterUrl { get; set; }

    /// <summary>
    /// Das vollständige Erscheinungsdatum.
    /// </summary>
    public DateTime? Erscheinungsdatum { get; set; }

    /// <summary>
    /// Die Altersfreigabe (z.B. FSK 12).
    /// </summary>
    public string? FskRating { get; set; }

    /// <summary>
    /// Das Genre oder die Kategorie des Films.
    /// </summary>
    public string? Genre { get; set; }

    /// <summary>
    /// Die durchschnittliche Nutzerwertung (0-10).
    /// </summary>
    public double? Nutzerwertung { get; set; }

    /// <summary>
    /// Eine Sammlung von Personen und ihren Eigenschaften, die an diesem Film mitgewirkt haben.
    /// </summary>
    public ICollection<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = [];
}
