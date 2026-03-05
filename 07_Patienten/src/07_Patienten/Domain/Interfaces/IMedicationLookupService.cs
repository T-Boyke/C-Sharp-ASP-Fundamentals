namespace _07_Patienten.Domain.Interfaces;

public class MedicationSearchResult
{
    public string Name { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Instructions { get; set; }
    public string Pzn { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

public interface IMedicationLookupService
{
    Task<MedicationSearchResult> SearchByPznAsync(string pzn);
    Task<List<MedicationSearchResult>> SearchAsync(string query);
}
