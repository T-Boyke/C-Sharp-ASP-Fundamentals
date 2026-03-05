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
            "07125301" => new MedicationSearchResult { Name = "ACC Akut 600", Dosage = "600mg", Instructions = "In einem Glas Wasser auflösen.", Pzn = pzn, Success = true },
            "02537446" => new MedicationSearchResult { Name = "Diclofenac-ratiopharm 50 mg", Dosage = "50mg", Instructions = "Max. 3x täglich nach den Mahlzeiten.", Pzn = pzn, Success = true },
            "00101683" => new MedicationSearchResult { Name = "Novaminsulfon 500 mg Lichtenstein", Dosage = "500mg", Instructions = "Gegen starke Schmerzen/Fieber.", Pzn = pzn, Success = true },
            "00762956" => new MedicationSearchResult { Name = "Metoprololsuccinat 47,5 mg", Dosage = "47,5mg", Instructions = "Morgens unzerkaut einnehmen.", Pzn = pzn, Success = true },
            "01026414" => new MedicationSearchResult { Name = "Ramipril 5 mg - 1 A Pharma", Dosage = "5mg", Instructions = "Täglich zur gleichen Zeit.", Pzn = pzn, Success = true },
            "02143003" => new MedicationSearchResult { Name = "Bisoprolol 5 mg ratiopharm", Dosage = "5mg", Instructions = "Blutdrucksenker morgens einnehmen.", Pzn = pzn, Success = true },
            "01848529" => new MedicationSearchResult { Name = "L-Thyroxin 100 Henning", Dosage = "100µg", Instructions = "30 Min. vor dem Frühstück.", Pzn = pzn, Success = true },
            "03290453" => new MedicationSearchResult { Name = "Metformin 500 mg - 1 A Pharma", Dosage = "500mg", Instructions = "Zu oder nach den Mahlzeiten.", Pzn = pzn, Success = true },
            "00032338" => new MedicationSearchResult { Name = "Simvastatin 20 mg ratiopharm", Dosage = "20mg", Instructions = "Abends einnehmen.", Pzn = pzn, Success = true },
            "04000300" => new MedicationSearchResult { Name = "Amlodipin 5 mg - 1 A Pharma", Dosage = "5mg", Instructions = "Täglich 1 Tablette morgens.", Pzn = pzn, Success = true },
            "01130386" => new MedicationSearchResult { Name = "Candesartan 8 mg ratiopharm", Dosage = "8mg", Instructions = "Unabhängig von den Mahlzeiten.", Pzn = pzn, Success = true },
            "02078652" => new MedicationSearchResult { Name = "Atorvastatin 20 mg - 1 A Pharma", Dosage = "20mg", Instructions = "Einmal täglich abends.", Pzn = pzn, Success = true },
            "04953496" => new MedicationSearchResult { Name = "Pantoprazol 20 mg ratiopharm", Dosage = "20mg", Instructions = "Morgens vor dem Essen.", Pzn = pzn, Success = true },
            "00746680" => new MedicationSearchResult { Name = "Spironolacton 50 mg ratiopharm", Dosage = "50mg", Instructions = "Zum Frühstück einnehmen.", Pzn = pzn, Success = true },
            "00882650" => new MedicationSearchResult { Name = "Furosemid 40 mg ratiopharm", Dosage = "40mg", Instructions = "Entwässerungsmittel morgens.", Pzn = pzn, Success = true },
            "00133282" => new MedicationSearchResult { Name = "Prednisolon 5 mg Galen", Dosage = "5mg", Instructions = "Zwischen 6:00 und 8:00 Uhr morgens.", Pzn = pzn, Success = true },
            "01026549" => new MedicationSearchResult { Name = "Venlafaxin 75 mg - 1 A Pharma", Dosage = "75mg", Instructions = "Immer zur gleichen Tageszeit.", Pzn = pzn, Success = true },
            "02147320" => new MedicationSearchResult { Name = "Sertralin 50 mg ratiopharm", Dosage = "50mg", Instructions = "Morgens oder abends einnehmen.", Pzn = pzn, Success = true },
            "02534821" => new MedicationSearchResult { Name = "Citalopram 20 mg ratiopharm", Dosage = "20mg", Instructions = "Einmal täglich morgens oder abends.", Pzn = pzn, Success = true },
            "03000572" => new MedicationSearchResult { Name = "Escitalopram 10 mg - 1 A Pharma", Dosage = "10mg", Instructions = "Unabhängig von der Nahrungsaufnahme.", Pzn = pzn, Success = true },
            "04414163" => new MedicationSearchResult { Name = "Mirtazapin 15 mg ratiopharm", Dosage = "15mg", Instructions = "Abends vor dem Schlafengehen.", Pzn = pzn, Success = true },
            "00723821" => new MedicationSearchResult { Name = "Lorazepam 1 mg dura", Dosage = "1mg", Instructions = "Bei akuten Angstzuständen.", Pzn = pzn, Success = true },
            "01242005" => new MedicationSearchResult { Name = "Diazepam 5 mg - 1 A Pharma", Dosage = "5mg", Instructions = "Abends zur Beruhigung.", Pzn = pzn, Success = true },
            "02142469" => new MedicationSearchResult { Name = "Ibuprofen 600 mg ratiopharm", Dosage = "600mg", Instructions = "Verschreibungspflichtig bei Entzündungen.", Pzn = pzn, Success = true },
            "00101693" => new MedicationSearchResult { Name = "Metamizol 500 mg ratiopharm", Dosage = "500mg", Instructions = "Bei starken Schmerzen einnehmen.", Pzn = pzn, Success = true },
            "04000100" => new MedicationSearchResult { Name = "Torasemid 5 mg - 1 A Pharma", Dosage = "5mg", Instructions = "Morgens zur Entwässerung.", Pzn = pzn, Success = true },
            _ => null
        };
    }
}
