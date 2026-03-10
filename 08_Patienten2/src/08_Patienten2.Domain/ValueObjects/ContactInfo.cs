namespace _08_Patienten2.Domain.ValueObjects;

/// <summary>
/// Repräsentiert Kontaktinformationen als unveränderliches Wertobjekt (Value Object).
/// </summary>
/// <param name="Email">Die E-Mail-Adresse des Patienten.</param>
/// <param name="Phone">Die Telefonnummer für Rückfragen.</param>
public record ContactInfo(string Email, string Phone);
