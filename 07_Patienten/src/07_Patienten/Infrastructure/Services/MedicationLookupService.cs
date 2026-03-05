using System.Text.Json;
using _07_Patienten.Domain.Interfaces;

namespace _07_Patienten.Infrastructure.Services;

/// <summary>
/// Medikamenten-Suche aus einer statischen Datenbank mit 340+ deutschen Arzneimitteln.
/// Die Daten werden beim Programmstart aus /Data/medications.json geladen.
/// </summary>
public class MedicationLookupService : IMedicationLookupService
{
    private static List<MedicationSearchResult> _medications = new();
    private static bool _loaded;
    private static readonly object _lock = new();

    public MedicationLookupService(IWebHostEnvironment env)
    {
        if (!_loaded)
        {
            lock (_lock)
            {
                if (!_loaded)
                {
                    var path = Path.Combine(env.ContentRootPath, "Data", "medications.json");
                    if (File.Exists(path))
                    {
                        var json = File.ReadAllText(path);
                        var items = JsonSerializer.Deserialize<List<MedEntry>>(json,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (items != null)
                        {
                            _medications = items.Select(e => new MedicationSearchResult
                            {
                                Name = e.Name,
                                Dosage = e.Dosage,
                                Instructions = e.Instructions,
                                Pzn = e.Pzn,
                                Success = true
                            }).ToList();
                        }
                    }
                    _loaded = true;
                }
            }
        }
    }

    public Task<MedicationSearchResult> SearchByPznAsync(string pzn)
    {
        pzn = pzn.Replace(" ", "").Replace("-", "").Trim().PadLeft(8, '0');

        if (pzn.Length != 8 || !pzn.All(char.IsDigit))
            return Task.FromResult(new MedicationSearchResult
            { Success = false, ErrorMessage = "Ungültige PZN (8 Ziffern)." });

        var result = _medications.FirstOrDefault(m => m.Pzn == pzn);
        return Task.FromResult(result ?? new MedicationSearchResult
        { Success = false, ErrorMessage = "PZN nicht in der Datenbank." });
    }

    public Task<List<MedicationSearchResult>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Task.FromResult(new List<MedicationSearchResult>());

        var q = query.Trim().ToLowerInvariant();
        var results = _medications
            .Where(m => m.Name.ToLowerInvariant().Contains(q) || m.Pzn.Contains(q))
            .Take(20) // Max 20 Ergebnisse für die UI
            .ToList();

        return Task.FromResult(results);
    }

    private record MedEntry(string Name, string Dosage, string Instructions, string Pzn);
}
