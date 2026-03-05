using System.Text.RegularExpressions;
using _07_Patienten.Domain.Interfaces;

namespace _07_Patienten.Infrastructure.Services;

public class MedicationLookupService : IMedicationLookupService
{
    private readonly HttpClient _httpClient;

    public MedicationLookupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "MedCare-Lookup-Service/1.0");
    }

    public async Task<MedicationSearchResult> SearchByPznAsync(string pzn)
    {
        // PZN Normalisierung (8 Stellen)
        pzn = pzn.Replace(" ", "").Replace("-", "").Trim().PadLeft(8, '0');

        if (pzn.Length != 8 || !pzn.All(char.IsDigit))
        {
            return new MedicationSearchResult { Success = false, ErrorMessage = "Ungültige PZN. Eine PZN muss 8 Ziffern enthalten." };
        }

        try
        {
            // Versuch 1: Bekannte Test-PZNs (Smart-Mock)
            var mockResult = GetMockResult(pzn);
            if (mockResult != null) return mockResult;

            // Versuch 2: Live-Abfrage via Shop-Apotheke (Leichtes Scraping für Demo-Zwecke)
            // Hinweis: Im echten Produkt würde man eine offizielle API wie ABDA nutzen.
            var searchUrl = $"https://www.shop-apotheke.com/suche.htm?q={pzn}";
            var response = await _httpClient.GetStringAsync(searchUrl);

            // Sehr einfaches Parsing des Titels oder der Produktdaten
            // Suchen nach Produktname im HTML
            var match = Regex.Match(response, @"<title>(.*?) - shop-apotheke\.com</title>", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var fullTitle = match.Groups[1].Value.Trim();
                // Oft ist der Titel "Produktname, Dosierung, Packungsgröße"
                var parts = fullTitle.Split(',');
                
                return new MedicationSearchResult
                {
                    Name = parts[0].Trim(),
                    Dosage = parts.Length > 1 ? parts[1].Trim() : "Daten laut PZN",
                    Instructions = "Einnahme laut Packungsbeilage",
                    Pzn = pzn,
                    Success = true
                };
            }

            return new MedicationSearchResult { Success = false, ErrorMessage = "Medikament konnte nicht gefunden werden." };
        }
        catch (Exception ex)
        {
            return new MedicationSearchResult { Success = false, ErrorMessage = $"Fehler bei der Live-Abfrage: {ex.Message}" };
        }
    }

    private MedicationSearchResult? GetMockResult(string pzn)
    {
        return pzn switch
        {
            "00951474" => new MedicationSearchResult { Name = "Ibuprofen 400 AKUT", Dosage = "400mg", Instructions = "Bei Schmerzen 1 Tbl. unzerkaut einnehmen.", Pzn = pzn, Success = true },
            "01242301" => new MedicationSearchResult { Name = "Paracetamol 500 - 1 A Pharma", Dosage = "500mg", Instructions = "Max. 4x täglich 1 Tablette.", Pzn = pzn, Success = true },
            "01580221" => new MedicationSearchResult { Name = "ASPIRIN 500 mg Tabletten", Dosage = "500mg", Instructions = "Nach dem Essen mit reichlich Wasser.", Pzn = pzn, Success = true },
            "04414571" => new MedicationSearchResult { Name = "OMEPRAZOL ratiopharm 20 mg", Dosage = "20mg", Instructions = "Morgens nüchtern einnehmen.", Pzn = pzn, Success = true },
            _ => null
        };
    }
}
