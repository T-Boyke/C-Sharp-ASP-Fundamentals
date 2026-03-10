using _09_Identity.Domain.ValueObjects;

namespace _09_Identity.Domain.Interfaces;

/// <summary>
/// Domain-Schnittstelle für Authentifizierungs-Operationen.
/// </summary>
public interface IAuthService
{
    Task<bool> LoginAsync(Credentials credentials);
    Task LogoutAsync();
}
