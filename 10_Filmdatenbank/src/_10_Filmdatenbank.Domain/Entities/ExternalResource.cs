namespace _10_Filmdatenbank.Domain.Entities;

/// <summary>
/// Repräsentiert externe Ressourcen wie Amazon-Links, Merchandise oder offizielle Seiten.
/// </summary>
public class ExternalResource
{
    public int ExternalResourceID { get; set; }
    public int FilmID { get; set; }
    public Film Film { get; set; } = null!;

    /// <summary>
    /// Typ der Ressource (Amazon, Merchandise, PhysicalMedia, Soundtrack).
    /// </summary>
    public string Type { get; set; } = "Other";

    /// <summary>
    /// Anzeigename der Ressource.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Die URL zur Ressource.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Preisangabe (optional).
    /// </summary>
    public string? PriceHint { get; set; }
}
