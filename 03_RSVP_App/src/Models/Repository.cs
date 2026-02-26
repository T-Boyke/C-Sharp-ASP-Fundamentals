namespace Unit_03_RSVP_App.Models;

/// <summary>
/// Ein statisches Repository zur In-Memory-Speicherung von <see cref="GuestResponse"/> Objekten.
/// Simuliert eine Datenbank für die Dauer der Anwendungssitzung.
/// </summary>
/// <remarks>
/// Implementiert das Repository-Pattern in einer vereinfachten statischen Form.
/// Nicht geeignet für produktive Umgebungen mit Persistenzanforderungen (KISS-Prinzip).
/// </remarks>
public static class Repository
{
    /// <summary>
    /// Interne Liste zur Speicherung der Antworten.
    /// </summary>
    private static List<GuestResponse> responses = new();

    /// <summary>
    /// Ruft alle bisher eingegangenen Gast-Antworten ab.
    /// </summary>
    /// <value>Eine Enumeration von <see cref="GuestResponse"/> Objekten.</value>
    public static IEnumerable<GuestResponse> Responses => responses;

    /// <summary>
    /// Fügt dem Repository eine neue Antwort hinzu.
    /// </summary>
    /// <param name="response">Das zu speichernde <see cref="GuestResponse"/> Objekt.</param>
    /// <remarks>
    /// Die Methode ist thread-safe in diesem Kontext nicht garantiert, 
    /// reicht aber für Demonstrationszwecke aus.
    /// </remarks>
    /// <example>
    /// <code>
    /// Repository.AddResponse(new GuestResponse { Name = "Erika" });
    /// </code>
    /// </example>
    public static void AddResponse(GuestResponse response)
    {
        responses.Add(response);
    }
}
