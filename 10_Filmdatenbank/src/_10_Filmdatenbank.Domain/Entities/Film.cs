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
    /// Eine Sammlung von Personen und ihren Eigenschaften, die an diesem Film mitgewirkt haben.
    /// </summary>
    public ICollection<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = [];
}
