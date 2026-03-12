using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace _10_Filmdatenbank.Infrastructure.Persistence;

/// <summary>
/// Werkzeug für die Erstellung des Datenbankkontexts zur Entwurfszeit (z.B. für Migrationen).
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <summary>
    /// Erstellt eine neue Instanz des Datenbankkontexts.
    /// </summary>
    /// <param name="args">Argumente für die Erstellung.</param>
    /// <returns>Eine konfigurierte Instanz von ApplicationDbContext.</returns>
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Filmdatenbank;Trusted_Connection=True;MultipleActiveResultSets=true", 
            b => b.MigrationsAssembly("_10_Filmdatenbank.Infrastructure"));

        return new ApplicationDbContext(builder.Options);
    }
}
