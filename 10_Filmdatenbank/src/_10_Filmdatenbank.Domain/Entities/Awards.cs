using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert eine Auszeichnung oder Nominierung für eine Person.
/// </summary>
public class PersonAward
{
    /// <summary>
    /// Eindeutige ID der Auszeichnung.
    /// </summary>
    [Key]
    public int AwardID { get; set; }

    /// <summary>
    /// Name der Auszeichnung (z.B. Oscar, Golden Globe).
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategorie der Auszeichnung (z.B. Bester Hauptdarsteller).
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Das Jahr der Auszeichnung.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Gibt an, ob die Auszeichnung gewonnen wurde oder nur eine Nominierung war.
    /// </summary>
    public bool IsWin { get; set; }

    /// <summary>
    /// Fremdschlüssel zur Person.
    /// </summary>
    public int PersonID { get; set; }

    /// <summary>
    /// Navigations-Eigenschaft zur Person.
    /// </summary>
    [ForeignKey("PersonID")]
    public virtual Person Person { get; set; } = null!;
}

/// <summary>
/// Repräsentiert eine Auszeichnung oder Nominierung für einen Film.
/// </summary>
public class FilmAward
{
    /// <summary>
    /// Eindeutige ID der Auszeichnung.
    /// </summary>
    [Key]
    public int AwardID { get; set; }

    /// <summary>
    /// Name der Auszeichnung.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategorie der Auszeichnung.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Das Jahr der Auszeichnung.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Gibt an, ob gewonnen oder nominiert.
    /// </summary>
    public bool IsWin { get; set; }

    /// <summary>
    /// Fremdschlüssel zum Film.
    /// </summary>
    public int FilmID { get; set; }

    /// <summary>
    /// Navigations-Eigenschaft zum Film.
    /// </summary>
    [ForeignKey("FilmID")]
    public virtual Film Film { get; set; } = null!;
}

/// <summary>
/// Repräsentiert eine Auszeichnung für ein Produktionsstudio.
/// </summary>
public class ProductionCompanyAward
{
    /// <summary>
    /// Eindeutige ID der Auszeichnung.
    /// </summary>
    [Key]
    public int AwardID { get; set; }

    /// <summary>
    /// Name der Auszeichnung.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Kategorie der Auszeichnung.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Das Jahr der Auszeichnung.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Gibt an, ob gewonnen oder nominiert.
    /// </summary>
    public bool IsWin { get; set; }

    /// <summary>
    /// Fremdschlüssel zum Studio.
    /// </summary>
    public int ProductionCompanyID { get; set; }

    /// <summary>
    /// Navigations-Eigenschaft zum Studio.
    /// </summary>
    [ForeignKey("ProductionCompanyID")]
    public virtual ProductionCompany ProductionCompany { get; set; } = null!;
}
