using System;
using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert eine länderspezifische Veröffentlichung (TMDB Releases).
/// Beinhaltet Daten wie FSK-Rating und lokales Release-Datum.
/// </summary>
public class FilmRelease
{
    /// <summary>
    /// Die eindeutige Kennung des Release-Eintrags.
    /// </summary>
    public int ReleaseID { get; set; }

    /// <summary>
    /// Die ID des zugehörigen Films.
    /// </summary>
    public int FilmID { get; set; }

    /// <summary>
    /// Der Film, zu dem dieser Release-Eintrag gehört.
    /// </summary>
    public Film Film { get; set; } = null!;

    /// <summary>
    /// Der Ländercode (ISO 3166-1).
    /// </summary>
    [Required]
    [StringLength(2)]
    public string Iso_3166_1 { get; set; } = string.Empty;

    /// <summary>
    /// Die Altersfreigabe (z.B. FSK 12, R, PG-13).
    /// </summary>
    public string? Certification { get; set; }

    /// <summary>
    /// Das länderspezifische Release-Datum.
    /// </summary>
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Der Typ des Releases (1=Premiere, 2=Theatrical (limited), 3=Theatrical, 4=Digital, 5=Physical, 6=TV).
    /// </summary>
    public int Type { get; set; }
}
