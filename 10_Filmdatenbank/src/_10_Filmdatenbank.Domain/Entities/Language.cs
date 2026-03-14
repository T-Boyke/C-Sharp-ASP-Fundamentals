using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert eine Sprache nach ISO 639-1.
/// </summary>
public class Language
{
    /// <summary>
    /// Der ISO 639-1 Sprachcode (z.B. de, en) - Primärschlüssel.
    /// </summary>
    [Key]
    [StringLength(2, MinimumLength = 2)]
    public string Iso639_1 { get; set; } = string.Empty;

    /// <summary>
    /// Der Name der Sprache (in Englisch).
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Der Name der Sprache in der Landessprache.
    /// </summary>
    public string? NativeName { get; set; }

    /// <summary>
    /// Filme, in denen diese Sprache gesprochen wird.
    /// </summary>
    public ICollection<Film> SpokenInFilms { get; set; } = [];
}
