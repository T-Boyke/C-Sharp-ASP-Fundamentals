using _07_Patienten.Domain.Entities;

namespace _07_Patienten.Domain.Interfaces;

/// <summary>
/// Interface für den Datenzugriff auf Medikamente.
/// </summary>
public interface IMedicationRepository
{
    Task<IEnumerable<Medication>> GetByPatientIdAsync(int patientId);
    Task AddAsync(Medication medication);
    Task DeleteAsync(int id);
}
