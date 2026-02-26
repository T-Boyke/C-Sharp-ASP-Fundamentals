using System.ComponentModel.DataAnnotations;

namespace Unit_03_RSVP_App.Models;

/// <summary>
/// Repräsentiert die Antwort eines Gastes auf eine Einladung (RSVP).
/// Diese Klasse dient als Datenmodell für das RSVP-Formular und demonstriert
/// die Verwendung von Data Annotations zur Validierung.
/// </summary>
/// <remarks>
/// Die Validierung erfolgt sowohl clientseitig (via jQuery Unobtrusive) 
/// als auch serverseitig im Controller über <c>ModelState.IsValid</c>.
/// </remarks>
/// <example>
/// <code>
/// var response = new GuestResponse { 
///     Name = "Max Mustermann", 
///     Email = "max@example.com", 
///     WillAttend = true 
/// };
/// </code>
/// </example>
public class GuestResponse
{
    /// <summary>
    /// Ruft den Namen des Gastes ab oder legt diesen fest.
    /// Pflichtfeld für die Zuordnung der Antwort.
    /// </summary>
    [Required(ErrorMessage = "Bitte geben Sie Ihren Namen ein")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ruft die E-Mail-Adresse des Gastes ab oder legt diese fest.
    /// Wird für die Bestätigung und Rückfragen benötigt.
    /// </summary>
    /// <exception cref="ValidationException">Wird ausgelöst, wenn das Format ungültig ist.</exception>
    [Required(ErrorMessage = "Bitte geben Sie Ihre E-Mail-Adresse ein")]
    [EmailAddress(ErrorMessage = "Bitte geben Sie eine gültige E-Mail-Adresse ein")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Ruft die Telefonnummer des Gastes ab oder legt diese fest.
    /// Optionales Feld für kurzfristige Erreichbarkeit.
    /// </summary>
    [Required(ErrorMessage = "Bitte geben Sie Ihre Telefonnummer ein")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Ruft ab oder legt fest, ob der Gast an der Veranstaltung teilnehmen wird.
    /// Ein <c>bool?</c> (nullable) wird verwendet, um den "Nicht ausgewählt"-Status im Formular zu ermöglichen.
    /// </summary>
    [Required(ErrorMessage = "Bitte geben Sie an, ob Sie teilnehmen werden")]
    public bool? WillAttend { get; set; }
}
