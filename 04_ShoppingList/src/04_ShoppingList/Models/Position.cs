namespace _04_ShoppingList.Models;

/// <summary>
/// Stellt eine einzelne Position (einen Artikel) auf der Einkaufsliste dar.
/// </summary>
public class Position
{
    /// <summary>
    /// Eindeutige ID zur Identifikation des Artikels (wichtig zum Löschen).
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Der Name des zu besorgenden Artikels (z.B. "Milch").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Die Anzahl der benötigten Artikel.
    /// </summary>
    public int Anzahl { get; set; }

    /// <summary>
    /// Das Geschäft, in dem der Artikel gekauft werden soll.
    /// </summary>
    public string Geschaeft { get; set; } = string.Empty;
}

