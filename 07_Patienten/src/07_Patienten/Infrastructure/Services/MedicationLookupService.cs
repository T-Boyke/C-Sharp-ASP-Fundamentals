using System.Text.Json;
using System.Text.RegularExpressions;
using _07_Patienten.Domain.Interfaces;

namespace _07_Patienten.Infrastructure.Services;

public class MedicationLookupService : IMedicationLookupService
{
    private readonly HttpClient _httpClient;

    // Thread-safe lokaler Cache (wird zur Laufzeit erweitert)
    private static readonly List<MedicationSearchResult> _cache = new(MockDatabase);
    private static readonly object _cacheLock = new();

    public MedicationLookupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MedCare-Lookup-Service/1.0");
    }

    public async Task<MedicationSearchResult> SearchByPznAsync(string pzn)
    {
        pzn = pzn.Replace(" ", "").Replace("-", "").Trim().PadLeft(8, '0');

        if (pzn.Length != 8 || !pzn.All(char.IsDigit))
        {
            return new MedicationSearchResult { Success = false, ErrorMessage = "Ungültige PZN. Eine PZN muss 8 Ziffern enthalten." };
        }

        // Zuerst im lokalen Cache suchen
        var cached = GetFromCache(pzn);
        if (cached != null) return cached;

        return new MedicationSearchResult { Success = false, ErrorMessage = "Medikament nicht gefunden." };
    }

    /// <summary>
    /// Sucht Medikamente im lokalen Cache. Falls wenig Treffer, wird die OpenFDA API
    /// im Hintergrund abgefragt und die Ergebnisse lokal gecacht.
    /// </summary>
    public async Task<List<MedicationSearchResult>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<MedicationSearchResult>();

        query = query.Trim();
        var lowerQuery = query.ToLowerInvariant();

        // 1. Lokale Ergebnisse
        List<MedicationSearchResult> localResults;
        lock (_cacheLock)
        {
            localResults = _cache
                .Where(m => m.Name.ToLowerInvariant().Contains(lowerQuery) || m.Pzn.Contains(lowerQuery))
                .ToList();
        }

        // 2. Falls weniger als 3 lokale Treffer → OpenFDA API abfragen
        if (localResults.Count < 3)
        {
            try
            {
                var fdaResults = await SearchOpenFdaAsync(query);
                if (fdaResults.Any())
                {
                    // Neue Ergebnisse in den Cache aufnehmen
                    lock (_cacheLock)
                    {
                        foreach (var r in fdaResults)
                        {
                            if (!_cache.Any(c => c.Name.Equals(r.Name, StringComparison.OrdinalIgnoreCase)))
                            {
                                _cache.Add(r);
                            }
                        }
                    }

                    // Lokale Ergebnisse aktualisieren
                    lock (_cacheLock)
                    {
                        localResults = _cache
                            .Where(m => m.Name.ToLowerInvariant().Contains(lowerQuery) || m.Pzn.Contains(lowerQuery))
                            .ToList();
                    }
                }
            }
            catch
            {
                // Wenn die API nicht erreichbar ist, verwenden wir nur den lokalen Cache
            }
        }

        return localResults;
    }

    /// <summary>
    /// Durchsucht die OpenFDA Drug Label API nach Medikamenten.
    /// </summary>
    private async Task<List<MedicationSearchResult>> SearchOpenFdaAsync(string query)
    {
        var results = new List<MedicationSearchResult>();
        var url = $"https://api.fda.gov/drug/label.json?search=openfda.brand_name:{Uri.EscapeDataString(query)}&limit=10";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return results;

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("results", out var resultsArray))
            return results;

        foreach (var item in resultsArray.EnumerateArray())
        {
            try
            {
                string name = "Unbekannt";
                string dosage = "";

                // Name aus openfda.brand_name extrahieren
                if (item.TryGetProperty("openfda", out var openfda))
                {
                    if (openfda.TryGetProperty("brand_name", out var brandNames) && brandNames.GetArrayLength() > 0)
                    {
                        name = brandNames[0].GetString() ?? "Unbekannt";
                    }

                    // Dosierung aus dosage_form extrahieren
                    if (openfda.TryGetProperty("dosage_form", out var dosageForms) && dosageForms.GetArrayLength() > 0)
                    {
                        dosage = dosageForms[0].GetString() ?? "";
                    }

                    // Stärke aus openfda.substance_name 
                    if (openfda.TryGetProperty("product_type", out var productType) && productType.GetArrayLength() > 0)
                    {
                        var type = productType[0].GetString();
                        if (type == "HUMAN PRESCRIPTION DRUG")
                            dosage += " (Rx)";
                    }
                }

                // Duplikate nach Name vermeiden
                if (results.Any(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                    continue;

                results.Add(new MedicationSearchResult
                {
                    Name = name,
                    Dosage = dosage,
                    Instructions = "Einnahme laut Packungsbeilage",
                    Pzn = "", // OpenFDA hat keine PZN (deutsche Nummern)
                    Success = true
                });
            }
            catch
            {
                // Einzelnen fehlerhaften Eintrag überspringen
            }
        }

        return results;
    }

    private MedicationSearchResult? GetFromCache(string pzn)
    {
        lock (_cacheLock)
        {
            return _cache.FirstOrDefault(m => m.Pzn == pzn);
        }
    }

    // Statische Seed-Daten (deutsches Referenz-Sortiment)
    private static readonly List<MedicationSearchResult> MockDatabase = new()
    {
        new() { Name = "Ibuprofen 400 AKUT", Dosage = "400mg", Instructions = "Bei Schmerzen 1 Tbl. unzerkaut einnehmen.", Pzn = "00951474", Success = true },
        new() { Name = "Paracetamol 500 - 1 A Pharma", Dosage = "500mg", Instructions = "Max. 4x täglich 1 Tablette.", Pzn = "01242301", Success = true },
        new() { Name = "ASPIRIN 500 mg Tabletten", Dosage = "500mg", Instructions = "Nach dem Essen mit reichlich Wasser.", Pzn = "01580221", Success = true },
        new() { Name = "OMEPRAZOL ratiopharm 20 mg", Dosage = "20mg", Instructions = "Morgens nüchtern einnehmen.", Pzn = "04414571", Success = true },
        new() { Name = "ACC Akut 600", Dosage = "600mg", Instructions = "In einem Glas Wasser auflösen.", Pzn = "07125301", Success = true },
        new() { Name = "Diclofenac-ratiopharm 50 mg", Dosage = "50mg", Instructions = "Max. 3x täglich nach den Mahlzeiten.", Pzn = "02537446", Success = true },
        new() { Name = "Novaminsulfon 500 mg Lichtenstein", Dosage = "500mg", Instructions = "Gegen starke Schmerzen/Fieber.", Pzn = "00101683", Success = true },
        new() { Name = "Metoprololsuccinat 47,5 mg", Dosage = "47,5mg", Instructions = "Morgens unzerkaut einnehmen.", Pzn = "00762956", Success = true },
        new() { Name = "Ramipril 5 mg - 1 A Pharma", Dosage = "5mg", Instructions = "Täglich zur gleichen Zeit.", Pzn = "01026414", Success = true },
        new() { Name = "Bisoprolol 5 mg ratiopharm", Dosage = "5mg", Instructions = "Blutdrucksenker morgens einnehmen.", Pzn = "02143003", Success = true },
        new() { Name = "L-Thyroxin 100 Henning", Dosage = "100µg", Instructions = "30 Min. vor dem Frühstück.", Pzn = "01848529", Success = true },
        new() { Name = "Metformin 500 mg - 1 A Pharma", Dosage = "500mg", Instructions = "Zu oder nach den Mahlzeiten.", Pzn = "03290453", Success = true },
        new() { Name = "Simvastatin 20 mg ratiopharm", Dosage = "20mg", Instructions = "Abends einnehmen.", Pzn = "00032338", Success = true },
        new() { Name = "Amlodipin 5 mg - 1 A Pharma", Dosage = "5mg", Instructions = "Täglich 1 Tablette morgens.", Pzn = "04000300", Success = true },
        new() { Name = "Candesartan 8 mg ratiopharm", Dosage = "8mg", Instructions = "Unabhängig von den Mahlzeiten.", Pzn = "01130386", Success = true },
        new() { Name = "Atorvastatin 20 mg - 1 A Pharma", Dosage = "20mg", Instructions = "Einmal täglich abends.", Pzn = "02078652", Success = true },
        new() { Name = "Pantoprazol 20 mg ratiopharm", Dosage = "20mg", Instructions = "Morgens vor dem Essen.", Pzn = "04953496", Success = true },
        new() { Name = "Spironolacton 50 mg ratiopharm", Dosage = "50mg", Instructions = "Zum Frühstück einnehmen.", Pzn = "00746680", Success = true },
        new() { Name = "Furosemid 40 mg ratiopharm", Dosage = "40mg", Instructions = "Entwässerungsmittel morgens.", Pzn = "00882650", Success = true },
        new() { Name = "Prednisolon 5 mg Galen", Dosage = "5mg", Instructions = "Zwischen 6:00 und 8:00 Uhr morgens.", Pzn = "00133282", Success = true },
        new() { Name = "Venlafaxin 75 mg - 1 A Pharma", Dosage = "75mg", Instructions = "Immer zur gleichen Tageszeit.", Pzn = "01026549", Success = true },
        new() { Name = "Sertralin 50 mg ratiopharm", Dosage = "50mg", Instructions = "Morgens oder abends einnehmen.", Pzn = "02147320", Success = true },
        new() { Name = "Citalopram 20 mg ratiopharm", Dosage = "20mg", Instructions = "Einmal täglich morgens oder abends.", Pzn = "02534821", Success = true },
        new() { Name = "Escitalopram 10 mg - 1 A Pharma", Dosage = "10mg", Instructions = "Unabhängig von der Nahrungsaufnahme.", Pzn = "03000572", Success = true },
        new() { Name = "Mirtazapin 15 mg ratiopharm", Dosage = "15mg", Instructions = "Abends vor dem Schlafengehen.", Pzn = "04414163", Success = true },
        new() { Name = "Lorazepam 1 mg dura", Dosage = "1mg", Instructions = "Bei akuten Angstzuständen.", Pzn = "00723821", Success = true },
        new() { Name = "Diazepam 5 mg - 1 A Pharma", Dosage = "5mg", Instructions = "Abends zur Beruhigung.", Pzn = "01242005", Success = true },
        new() { Name = "Ibuprofen 600 mg ratiopharm", Dosage = "600mg", Instructions = "Verschreibungspflichtig bei Entzündungen.", Pzn = "02142469", Success = true },
        new() { Name = "Metamizol 500 mg ratiopharm", Dosage = "500mg", Instructions = "Bei starken Schmerzen einnehmen.", Pzn = "00101693", Success = true },
        new() { Name = "Torasemid 5 mg - 1 A Pharma", Dosage = "5mg", Instructions = "Morgens zur Entwässerung.", Pzn = "04000100", Success = true },
        new() { Name = "Tramadol 50 mg - 1 A Pharma", Dosage = "50mg", Instructions = "Bei Bedarf gegen starke Schmerzen.", Pzn = "03522327", Success = true },
        new() { Name = "Tramadol 100 mg retard ratiopharm", Dosage = "100mg", Instructions = "Alle 12 Stunden unzerkaut einnehmen.", Pzn = "02715643", Success = true }
    };
}
