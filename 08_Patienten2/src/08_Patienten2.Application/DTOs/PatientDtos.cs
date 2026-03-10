namespace _08_Patienten2.Application.DTOs;

/// <summary>
/// Datenübertragungsobjekt für die detaillierte Patientenanzeige.
/// </summary>
public record PatientDto(
    int Id,
    string Firstname,
    string Lastname,
    string Fullname,
    DateTime Birthdate,
    string SocialSecurityNumber,
    int Age,
    string? Symptoms
);

/// <summary>
/// DTO für die Erstellung eines neuen Patienten.
/// </summary>
public record PatientCreateDto(
    string Firstname,
    string Lastname,
    DateTime Birthdate,
    string SocialSecurityNumber
);
