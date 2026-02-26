namespace Unit_03_RSVP_App.Models;

/// <summary>
/// Ein vielseitiges Modell für UI-Komponenten (SFCs), das Flexibilität und Typsicherheit 
/// für Razor Partial Views bietet.
/// </summary>
public class ComponentModel
{
    /// <summary>Ruft das Label für das Formularfeld ab oder legt es fest.</summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>Ruft den Namen der Property im Zielmodell ab oder legt ihn fest.</summary>
    public string Property { get; set; } = string.Empty;

    /// <summary>Ruft den HTML-Eingabetyp ab oder legt ihn fest (Standard: "text").</summary>
    public string Type { get; set; } = "text";

    /// <summary>Ruft die Fehlermeldung bei Validierungsfehlern ab oder legt sie fest.</summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Ruft die Optionen für Select-Felder ab oder legt sie fest (optional).</summary>
    public object? Options { get; set; }

    /// <summary>Ruft den Button-Text ab oder legt ihn fest.</summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>Bestimmt, ob es sich um den primäre Aktion handelt.</summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>Ruft die CSS-Klasse für das Font-Awesome-Icon ab.</summary>
    public string? Icon { get; set; }
}
