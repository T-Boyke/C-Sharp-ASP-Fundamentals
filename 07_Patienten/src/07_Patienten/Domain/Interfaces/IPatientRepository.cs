using _07_Patienten.Domain.Entities;

namespace _07_Patienten.Domain.Interfaces;

/// <summary>
/// Repository-Interface für den Datenzugriff auf Patienten (DDD - Domain Interface).
/// </summary>
public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient?> GetByIdAsync(int id);
    Task<Patient?> GetByIdWithExaminationsAsync(int id);
    Task AddAsync(Patient patient);
    Task UpdateAsync(Patient patient);
    Task DeleteAsync(int id);
}
