using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert eine Produktionsfirma (Studio) von TMDB.
/// </summary>
public class ProductionCompany
{
    /// <summary>
    /// Die eindeutige Kennung der Firma in der lokalen Datenbank.
    /// </summary>
    public int ProductionCompanyID { get; set; }

    /// <summary>
    /// Die ID der Firma auf TMDB.
    /// </summary>
    [Required]
    public int TmdbId { get; set; }

    /// <summary>
    /// Der Name der Produktionsfirma.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Der Pfad zum Logo der Firma.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Das Herkunftsland der Firma (ISO 3166-1).
    /// </summary>
    public string? OriginCountry { get; set; }

    /// <summary>
    /// Eine Beschreibung der Produktionsfirma.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Der Hauptsitz der Firma.
    /// </summary>
    public string? Headquarters { get; set; }

    /// <summary>
    /// Die offizielle Homepage der Firma.
    /// </summary>
    public string? Homepage { get; set; }

    /// <summary>
    /// Die ID der Muttergesellschaft (falls vorhanden).
    /// </summary>
    public int? ParentCompanyID { get; set; }

    /// <summary>
    /// Die Muttergesellschaft.
    /// </summary>
    public ProductionCompany? ParentCompany { get; set; }

    /// <summary>
    /// Die strukturierten Auszeichnungen des Studios.
    /// </summary>
    public virtual ICollection<ProductionCompanyAward> ProductionCompanyAwards { get; set; } = new List<ProductionCompanyAward>();

    /// <summary>
    /// Die Filme, an deren Produktion diese Firma beteiligt war.
    /// </summary>
    public ICollection<Film> Films { get; set; } = [];
}
