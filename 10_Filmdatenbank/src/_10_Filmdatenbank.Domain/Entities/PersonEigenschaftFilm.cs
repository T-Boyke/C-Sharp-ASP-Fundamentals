using System;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Die Verknüpfungstabelle (Join-Table) zwischen Person, Eigenschaft und Film.
/// Definiert, in welcher Rolle eine Person an einem Film mitwirkt.
/// </summary>
public class PersonEigenschaftFilm
{
    /// <summary>
    /// Die eindeutige Kennung des Verknüpfungseintrags.
    /// </summary>
    public int PEFID { get; set; }
    
    /// <summary>
    /// Die Fremdschlüssel-ID der Person.
    /// </summary>
    public int PersonID { get; set; }
    
    /// <summary>
    /// Die referenzierte Person.
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Die Fremdschlüssel-ID des Films.
    /// </summary>
    public int FilmID { get; set; }
    
    /// <summary>
    /// Der referenzierte Film.
    /// </summary>
    public Film Film { get; set; } = null!;

    /// <summary>
    /// Die Fremdschlüssel-ID der Eigenschaft.
    /// </summary>
    public int EigenschaftID { get; set; }

    /// <summary>
    /// Die referenzierte Eigenschaft (Rolle).
    /// </summary>
    public Eigenschaft Eigenschaft { get; set; } = null!;

    /// <summary>
    /// Der spezifische Job (nur für Crew, z.B. Director).
    /// </summary>
    public string? Job { get; set; }

    /// <summary>
    /// Der Rollenname (nur für Schauspieler).
    /// </summary>
    public string? Character { get; set; }

    /// <summary>
    /// Die Abteilung (z.B. Directing, Production).
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// Die Reihenfolge in der Besetzungsliste.
    /// </summary>
    public int? Order { get; set; }
}
