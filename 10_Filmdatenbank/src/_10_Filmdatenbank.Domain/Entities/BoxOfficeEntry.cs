using System;

namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert einen Box-Office-Eintrag (Einspielergebnis) für einen Film.
/// </summary>
public class BoxOfficeEntry
{
    public int BoxOfficeEntryID { get; set; }
    public int FilmID { get; set; }
    public Film Film { get; set; } = null!;

    /// <summary>
    /// Das Datum oder der Zeitraum des Eintrags.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Die Einnahmen in diesem Zeitraum (USD).
    /// </summary>
    public long Revenue { get; set; }

    /// <summary>
    /// Das Land oder die Region (Global, Domestic, etc.).
    /// </summary>
    public string Region { get; set; } = "Global";

    /// <summary>
    /// Der Typ des Eintrags (Weekend, Cumulative, Opening).
    /// </summary>
    public string EntryType { get; set; } = "Cumulative";
}
