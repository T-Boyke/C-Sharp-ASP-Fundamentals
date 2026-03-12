using System.Threading.Tasks;

namespace _10_Filmdatenbank.Infrastructure.Persistence;

/// <summary>
/// Hilfsklasse zum Befüllen der Datenbank mit initialen Testdaten.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Befüllt die Datenbank mit initialen Daten (deaktiviert).
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    /// <returns>Ein Task-Objekt.</returns>
    public static Task SeedAsync(ApplicationDbContext context)
    {
        return Task.CompletedTask;
    }
}
