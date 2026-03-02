using System.Collections.Generic;
using System.Linq;

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
    /// Entfernt eine Position anhand ihrer eindeutigen Id aus der Liste.
    /// </summary>
    /// <param name="id">Die Id der zu löschenden Position.</param>
    /// <returns>True, wenn das Löschen erfolgreich war, ansonsten False.</returns>
    public static bool Delete(Guid id)
    {
        var itemToRemove = _positions.FirstOrDefault(p => p.Id == id);
        if (itemToRemove != null)
        {
            return _positions.Remove(itemToRemove);
        }
        return false;
    }

    /// <summary>
    /// Löscht alle gespeicherten Positionen. Nützlich für Unit Tests.
    /// </summary>
    public static void Clear()
    {
        _positions.Clear();
    }
}

