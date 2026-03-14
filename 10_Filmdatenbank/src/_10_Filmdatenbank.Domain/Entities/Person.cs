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
    /// Der Geburtsort der Person.
    /// </summary>
    public string? Geburtsort { get; set; }

    /// <summary>
    /// Tags oder Schlagworte für die Person.
    /// </summary>
    public string? Tags { get; set; }

    // --- TMDB PERFECT ALIGNMENT ---

    /// <summary>
    /// Die ID der Person auf TMDB.
    /// </summary>
    public int? TmdbId { get; set; }

    /// <summary>
    /// Die ID der Person auf IMDB.
    /// </summary>
    public string? ImdbId { get; set; }

    /// <summary>
    /// Das Geschlecht (1=Weiblich, 2=Männlich, 0=Unbekannt).
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// Das Todesdatum (falls verstorben).
    /// </summary>
    public DateTime? Deathday { get; set; }

    /// <summary>
    /// Die offizielle Homepage der Person.
    /// </summary>
    public string? Homepage { get; set; }

    /// <summary>
    /// Die TMDB-Popularitätsskala.
    /// </summary>
    public double? Popularity { get; set; }

    /// <summary>
    /// Informationen über gewonnene Auszeichnungen oder Nominierungen der Person.
    /// </summary>
    public string? Awards { get; set; }

    /// <summary>
    /// Der Bereich, für den die Person bekannt ist (z.B. acting, directing).
    /// </summary>
    public string? KnownForDepartment { get; set; }

    /// <summary>
    /// Alternative Namen oder Aliasse der Person (JSON).
    /// </summary>
    public string? AlsoKnownAs { get; set; }

    /// <summary>
    /// Gibt an, ob die Person in Inhalten für Erwachsene mitwirkt.
    /// </summary>
    public bool Adult { get; set; }

    /// <summary>
    /// Die ID der Person auf Wikidata.
    /// </summary>
    public string? WikidataId { get; set; }

    /// <summary>
    /// Die Facebook-ID der Person.
    /// </summary>
    public string? FacebookId { get; set; }

    /// <summary>
    /// Die Instagram-ID der Person.
    /// </summary>
    public string? InstagramId { get; set; }

    /// <summary>
    /// Die Twitter-ID der Person.
    /// </summary>
    public string? TwitterId { get; set; }

    /// <summary>
    /// Die Freebase-ID der Person.
    /// </summary>
    public string? FreebaseId { get; set; }

    /// <summary>
    /// Das Freebase-MID der Person.
    /// </summary>
    public string? FreebaseMid { get; set; }

    /// <summary>
    /// Die TV-Rage-ID der Person.
    /// </summary>
    public string? TvrageId { get; set; }

    /// <summary>
    /// Rohdaten der vollständigen Filmografie von TMDB (JSON).
    /// Enthält auch Filme, die nicht in der lokalen Datenbank vorhanden sind.
    /// </summary>
    public string? TmdbFilmographyJson { get; set; }

    /// <summary>
    /// Eine Sammlung von Filmen und Eigenschaften, an denen die Person mitgewirkt hat.
    /// </summary>
    public ICollection<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = [];

    /// <summary>
    /// Die strukturierten Auszeichnungen der Person.
    /// </summary>
    public virtual ICollection<PersonAward> AwardsList { get; set; } = new List<PersonAward>();
}
