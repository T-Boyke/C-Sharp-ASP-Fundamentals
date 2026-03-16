namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert einen Streaming-Anbieter oder eine Kaufplattform.
/// </summary>
public class WatchProvider
{
    public int WatchProviderID { get; set; }
    public int FilmID { get; set; }
    public Film Film { get; set; } = null!;

    /// <summary>
    /// Name des Anbieters (z.B. Netflix, Disney+, Amazon Prime).
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Typ des Angebots (flatrate, rent, buy).
    /// </summary>
    public string Type { get; set; } = "flatrate";

    /// <summary>
    /// URL zum Logo des Anbieters.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Direkter Link zum Stream (falls verfügbar).
    /// </summary>
    public string? WatchUrl { get; set; }

    /// <summary>
    /// Priorität der Anzeige.
    /// </summary>
    public int DisplayPriority { get; set; }
}
