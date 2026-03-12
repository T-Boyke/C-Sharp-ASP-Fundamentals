using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert ein Land nach ISO 3166-1.
/// </summary>
public class Country
{
    /// <summary>
    /// Der ISO 3166-1 Ländercode (z.B. DE, US) - Primärschlüssel.
    /// </summary>
    [Key]
    [StringLength(2, MinimumLength = 2)]
    public string Iso_3166_1 { get; set; } = string.Empty;

    /// <summary>
    /// Der Name des Landes (meist in Englisch).
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Der Name des Landes in der Landessprache.
    /// </summary>
    public string? NativeName { get; set; }

    /// <summary>
    /// Filme, die in diesem Land produziert wurden.
    /// </summary>
    public ICollection<Film> ProductionFilms { get; set; } = [];
}
