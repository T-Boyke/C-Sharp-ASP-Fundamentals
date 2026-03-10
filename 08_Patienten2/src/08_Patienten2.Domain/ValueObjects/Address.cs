namespace _08_Patienten2.Domain.ValueObjects;

/// <summary>
/// Repräsentiert eine physische Adresse als unveränderliches Wertobjekt (Value Object).
/// </summary>
/// <param name="Street">Die Straße und Hausnummer.</param>
/// <param name="ZipCode">Die Postleitzahl.</param>
/// <param name="City">Der Ort oder die Stadt.</param>
public record Address(string Street, string ZipCode, string City);
