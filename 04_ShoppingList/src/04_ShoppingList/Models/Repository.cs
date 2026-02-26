using System.Collections.Generic;

namespace _04_ShoppingList.Models;

/// <summary>
/// Statische Klasse zur in-memory Speicherung aller Positionen der Einkaufsliste.
/// Dient als temporäres Repository (Ersatz für eine Datenbank).
/// </summary>
public static class Repository
{
    private static List<Position> _positions = new();

    /// <summary>
    /// Gibt eine schreibgeschützte Liste aller bisher gespeicherten Positionen zurück.
    /// </summary>
    public static IEnumerable<Position> Positions => _positions;

    /// <summary>
    /// Fügt eine neue Position zur Einkaufsliste hinzu.
    /// </summary>
    /// <param name="position">Das hinzuzufügende Position-Objekt.</param>
    public static void AddResponse(Position position)
    {
        _positions.Add(position);
    }

    /// <summary>
    /// Löscht alle gespeicherten Positionen. Nützlich für Unit Tests.
    /// </summary>
    public static void Clear()
    {
        _positions.Clear();
    }
}

