using System;
using System.Collections.Generic;

namespace _04_ShoppingList.Models;

/// <summary>
/// Interface für die Speicherung aller Positionen der Einkaufsliste.
/// Ermöglicht Dependency Injection nach dem SOLID-Prinzip (Dependency Inversion).
/// </summary>
public interface IShoppingListRepository
{
    /// <summary>
    /// Gibt eine schreibgeschützte Liste aller bisher gespeicherten Positionen zurück.
    /// </summary>
    IEnumerable<Position> Positions { get; }

    /// <summary>
    /// Fügt eine neue Position zur Einkaufsliste hinzu.
    /// </summary>
    /// <param name="position">Das hinzuzufügende Position-Objekt.</param>
    void AddResponse(Position position);

    /// <summary>
    /// Gibt eine spezifische Position anhand ihrer ID zurück.
    /// </summary>
    Position? GetById(Guid id);

    /// <summary>
    /// Aktualisiert eine bestehende Position.
    /// </summary>
    bool Update(Position position);

    /// <summary>
    /// Entfernt eine Position anhand ihrer eindeutigen Id aus der Liste.
    /// </summary>
    /// <param name="id">Die Id der zu löschenden Position.</param>
    /// <returns>True, wenn das Löschen erfolgreich war, ansonsten False.</returns>
    bool Delete(Guid id);

    /// <summary>
    /// Löscht alle gespeicherten Positionen.
    /// </summary>
    void Clear();
}
