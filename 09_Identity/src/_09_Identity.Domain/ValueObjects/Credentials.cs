namespace _09_Identity.Domain.ValueObjects;

/// <summary>
/// Wertobjekt für Benutzer-Anmeldedaten.
/// </summary>
public record Credentials
{
    public string Username { get; }
    public string Password { get; }

    public Credentials(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Benutzername darf nicht leer sein.", nameof(username));
        
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Passwort darf nicht leer sein.", nameof(password));

        Username = username;
        Password = password;
    }
}
