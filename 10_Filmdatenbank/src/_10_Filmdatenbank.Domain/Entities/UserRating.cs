using System;
using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert eine individuelle Nutzerbewertung für einen Film in der CouchDB.
/// </summary>
public class UserRating
{
    /// <summary>
    /// Die eindeutige ID der Bewertung.
    /// </summary>
    [Key]
    public int UserRatingID { get; set; }

    /// <summary>
    /// Die ID des bewerteten Films.
    /// </summary>
    public int FilmID { get; set; }

    /// <summary>
    /// Der bewertete Film.
    /// </summary>
    public virtual Film Film { get; set; } = null!;

    /// <summary>
    /// Die ID des Benutzers, der die Bewertung abgegeben hat.
    /// </summary>
    public string UserID { get; set; } = string.Empty;

    /// <summary>
    /// Der Benutzer, der die Bewertung abgegeben hat.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// Der Wert der Bewertung (0.0 bis 10.0).
    /// </summary>
    [Range(0.0, 10.0)]
    public double Value { get; set; }

    /// <summary>
    /// Der Zeitpunkt, an dem die Bewertung erstellt oder zuletzt aktualisiert wurde.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
