using System;
using System.Collections.Generic;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert eine Rolle oder Eigenschaft (z.B. Regisseur, Schauspieler), die eine Person in einem Film einnimmt.
/// </summary>
public class Eigenschaft
{
    /// <summary>
    /// Die eindeutige Kennung der Eigenschaft.
    /// </summary>
    public int EigenschaftID { get; set; }

    /// <summary>
    /// Die Bezeichnung der Eigenschaft.
    /// </summary>
    public string Bezeichnung { get; set; } = string.Empty;

    /// <summary>
    /// Eine Sammlung der Verknüpfungen zwischen Personen und Filmen für diese Eigenschaft.
    /// </summary>
    public ICollection<PersonEigenschaftFilm> PersonEigenschaftFilme { get; set; } = [];
}
