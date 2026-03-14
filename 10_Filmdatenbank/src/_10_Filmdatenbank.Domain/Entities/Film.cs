using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    [Required]
    public string Titel { get; set; } = string.Empty;

    /// <summary>
    /// Das Erscheinungsjahr des Films.
    /// </summary>
    public int Erscheinungsjahr { get; set; }

    /// <summary>
    /// Die Spieldauer des Films in Minuten.
    /// </summary>
    [Range(1, 1000)]
    public int Spieldauer { get; set; }

    /// <summary>
    /// Der Preis des Films in Euro.
    /// </summary>
    [DataType(DataType.Currency)]
    [Range(0.01, 1000.00)]
    public decimal Preis { get; set; }

    /// <summary>
    /// Die detaillierte Handlung des Films.
    /// </summary>
    public string? Handlung { get; set; }

    /// <summary>
    /// Ein kurzer Werbespruch zum Film (Handled by Fanatic Metadata).
    /// </summary>

    /// <summary>
    /// Die URL zum Poster-Bild.
    /// </summary>
    public string? PosterUrl { get; set; }

    /// <summary>
    /// Das vollständige Erscheinungsdatum.
    /// </summary>
    public DateTime? Erscheinungsdatum { get; set; }

    /// <summary>
    /// Die durchschnittliche Nutzerwertung (0-10).
    /// </summary>

    /// <summary>
    /// Die durchschnittliche Nutzerwertung (0-10).
    /// </summary>
    public double? Nutzerwertung { get; set; }


    // --- TMDB PERFECT ALIGNMENT ---

    /// <summary>
    /// Die ID des Films auf TMDB.
    /// </summary>
    public int? TmdbId { get; set; }

    /// <summary>
    /// Die ID des Films auf IMDB.
    /// </summary>
    public string? ImdbId { get; set; }

    /// <summary>
    /// Der Originaltitel des Films.
    /// </summary>
    public string? OriginalTitle { get; set; }

    /// <summary>
    /// Der Werbespruch oder Slogan zum Film (Legacy Tagline).
    /// </summary>
    public string? Tagline { get; set; }

    /// <summary>
    /// Das Budget des Films in USD.
    /// </summary>
    public long? Budget { get; set; }

    /// <summary>
    /// Die Einnahmen des Films in USD.
    /// </summary>
    public long? Revenue { get; set; }

    /// <summary>
    /// Der Status der Produktion (z.B. Released).
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Die Laufzeit des Films in Minuten.
    /// </summary>
    public int? Runtime { get; set; }

    /// <summary>
    /// Die Originalsprache des Films (ISO 639-1).
    /// </summary>
    public string? OriginalLanguage { get; set; }

    /// <summary>
    /// Die offizielle Homepage des Films.
    /// </summary>
    public string? Homepage { get; set; }

    /// <summary>
    /// Die TMDB-Popularitätsskala.
    /// </summary>
    public double? Popularity { get; set; }

    /// <summary>
    /// Die Anzahl der Stimmen auf TMDB.
    /// </summary>
    public int? VoteCount { get; set; }

    /// <summary>
    /// Die durchschnittliche Nutzerwertung (0-10) von TMDB.
    /// </summary>
    public double? VoteAverage { get; set; }

    /// <summary>
    /// Die URL zum Hintergrundbild (Backdrop).
    /// </summary>
    public string? BackdropUrl { get; set; }

    /// <summary>
    /// Die ID des Films auf Wikidata.
    /// </summary>
    public string? WikidataId { get; set; }

    /// <summary>
    /// Die Facebook-ID des Films.
    /// </summary>
    public string? FacebookId { get; set; }

    /// <summary>
    /// Die Instagram-ID des Films.
    /// </summary>
    public string? InstagramId { get; set; }

    /// <summary>
    /// Die Twitter-ID des Films.
    /// </summary>
    public string? TwitterId { get; set; }

    /// <summary>
    /// Die Genres, die diesem Film zugeordnet sind.
    /// </summary>
    public ICollection<Genre> Genres { get; set; } = [];

    /// <summary>
    /// Die Schlagworte (Keywords) zum Film.
    /// </summary>
    public ICollection<Keyword> Keywords { get; set; } = [];

    /// <summary>
    /// Die Produktionsländer (ISO 3166-1).
    /// </summary>
    public ICollection<Country> ProductionCountries { get; set; } = [];

    /// <summary>
    /// Die im Film gesprochenen Sprachen (ISO 639-1).
    /// </summary>
    public ICollection<Language> SpokenLanguages { get; set; } = [];

    /// <summary>
    /// Alternative Titel des Films in verschiedenen Sprachen/Ländern.
    /// </summary>
    public ICollection<AlternativeTitle> AlternativeTitles { get; set; } = [];

    /// <summary>
    /// Länderspezifische Release-Informationen (Ratings, Daten).
    /// </summary>
    public ICollection<FilmRelease> Releases { get; set; } = [];

    /// <summary>
    /// Ähnliche Filme von TMDB.
    /// </summary>
    public ICollection<Film> SimilarFilms { get; set; } = [];

    /// <summary>
    /// Empfohlene Filme von TMDB.
    /// </summary>
    public ICollection<Film> RecommendedFilms { get; set; } = [];

    /// <summary>
    /// Die URL zum offiziellen Trailer (meist YouTube Embed URL).
    /// </summary>
    public string? TrailerUrl { get; set; }

    /// <summary>
    /// Informationen über gewonnene Auszeichnungen oder Nominierungen (Legacy Text).
    /// </summary>
    public string? Awards { get; set; }

    /// <summary>
    /// Die strukturierten Auszeichnungen des Films.
    /// </summary>
    public virtual ICollection<FilmAward> AwardsList { get; set; } = new List<FilmAward>();

    /// <summary>
    /// Gibt an, ob der Film nur für Erwachsene (Jugendschutz) geeignet ist.
    /// </summary>
    public bool Adult { get; set; }

    /// <summary>
    /// Die Fremdschlüssel-ID der Kollektion, zu der der Film gehört.
    /// </summary>
    public int? CollectionID { get; set; }

    /// <summary>
    /// Die Kollektion (Filmreihe), zu der dieser Film gehört.
    /// </summary>
    public Collection? Collection { get; set; }

    /// <summary>
    /// Die Produktionsfirmen, die an diesem Film beteiligt waren.
    /// </summary>
    public ICollection<ProductionCompany> ProductionCompanies { get; set; } = [];

    /// <summary>
    /// Eine Sammlung von Personen und ihren Eigenschaften, die an diesem Film mitgewirkt haben.
    /// </summary>
    public ICollection<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = [];
}
