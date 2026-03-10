namespace _09_Identity.Application.DTOs;

/// <summary>
/// DTO für den Login-Vorgang.
/// </summary>
public record LoginDto(string Username, string Password, string? ReturnUrl = "/");

/// <summary>
/// DTO für Benutzerinformationen.
/// </summary>
public record UserDto(string Id, string Username, string Email);
