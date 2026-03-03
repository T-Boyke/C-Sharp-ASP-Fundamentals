using System.Collections.Generic;
using System.Linq;

namespace _04_ShoppingList.Models;

/// <summary>
/// Repository zur in-memory Speicherung aller Positionen der Einkaufsliste.
/// Dient als temporäres Repository (Ersatz für eine Datenbank).
/// </summary>
public class Repository : IShoppingListRepository
{
    private readonly List<Position> _positions = new();

    /// <summary>
    /// Gibt eine schreibgeschützte Liste aller bisher gespeicherten Positionen zurück.
    /// </summary>
    public IEnumerable<Position> Positions => _positions;

    /// <summary>
    /// Fügt eine neue Position zur Einkaufsliste hinzu.
    /// </summary>
    /// <param name="position">Das hinzuzufügende Position-Objekt.</param>
    public void AddResponse(Position position)
    {
        _positions.Add(position);
    }

    /// <summary>
    /// Gibt eine spezifische Position anhand ihrer ID zurück.
    /// </summary>
    public Position? GetById(Guid id)
    {
        return _positions.FirstOrDefault(p => p.Id == id);
    }

    /// <summary>
    /// Aktualisiert eine bestehende Position.
    /// </summary>
    public bool Update(Position position)
    {
        var existing = _positions.FirstOrDefault(p => p.Id == position.Id);
        if (existing != null)
        {
            existing.Name = position.Name;
            existing.Anzahl = position.Anzahl;
            existing.Geschaeft = position.Geschaeft;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Entfernt eine Position anhand ihrer eindeutigen Id aus der Liste.
    /// </summary>
    /// <param name="id">Die Id der zu löschenden Position.</param>
    /// <returns>True, wenn das Löschen erfolgreich war, ansonsten False.</returns>
    public bool Delete(Guid id)
    {
        var itemToRemove = _positions.FirstOrDefault(p => p.Id == id);
        if (itemToRemove != null)
        {
            return _positions.Remove(itemToRemove);
        }
        return false;
    }

    /// <summary>
    /// Löscht alle gespeicherten Positionen.
    /// </summary>
    public void Clear()
    {
        _positions.Clear();
    }
}

