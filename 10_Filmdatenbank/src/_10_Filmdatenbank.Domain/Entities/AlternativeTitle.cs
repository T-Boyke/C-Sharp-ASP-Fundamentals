using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert einen alternativen Titel eines Films (TMDB Alternative Titles).
/// </summary>
public class AlternativeTitle
{
    /// <summary>
    /// Die eindeutige Kennung des alternativen Titels.
    /// </summary>
    public int AlternativeTitleID { get; set; }

    /// <summary>
    /// Die ID des zugehörigen Films.
    /// </summary>
    public int FilmID { get; set; }

    /// <summary>
    /// Der Film, zu dem dieser Titel gehört.
    /// </summary>
    public Film Film { get; set; } = null!;

    /// <summary>
    /// Der Ländercode (ISO 3166-1) für diesen Titel.
    /// </summary>
    [Required]
    [StringLength(2)]
    public string Iso3166_1 { get; set; } = string.Empty;

    /// <summary>
    /// Der alternative Titel.
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Der Typ des Titels (z.B. "Working Title").
    /// </summary>
    public string? Type { get; set; }
}
