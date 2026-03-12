using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert eine Person (z.B. Schauspieler, Regisseur), die an Filmen mitwirkt.
/// </summary>
public class Person
{
    /// <summary>
    /// Die eindeutige Kennung der Person.
    /// </summary>
    public int PersonID { get; set; }

    /// <summary>
    /// Der Vorname der Person.
    /// </summary>
    public string Vorname { get; set; } = string.Empty;

    /// <summary>
    /// Der Nachname der Person.
    /// </summary>
    public string Nachname { get; set; } = string.Empty;

    /// <summary>
    /// Ein Kurz-Portrait oder eine Biografie.
    /// </summary>
    public string? Biografie { get; set; }

    /// <summary>
    /// Die URL zum Profilbild.
    /// </summary>
    public string? ProfilBildUrl { get; set; }

    /// <summary>
    /// Das Geburtsdatum der Person.
    /// </summary>
    public DateTime? Geburtsdatum { get; set; }

    /// <summary>
    /// Eine Sammlung von Filmen und Eigenschaften, an denen die Person mitgewirkt hat.
    /// </summary>
    public ICollection<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = [];
}
