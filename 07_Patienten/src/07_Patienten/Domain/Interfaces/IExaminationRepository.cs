using _07_Patienten.Domain.Entities;

namespace _07_Patienten.Domain.Interfaces;

/// <summary>
/// Repository-Interface für den Datenzugriff auf Untersuchungen (DDD - Domain Interface).
/// </summary>
public interface IExaminationRepository
{
    Task<IEnumerable<Examination>> GetByPatientIdAsync(int patientId);
    Task<Examination?> GetByIdAsync(int id);
    Task AddAsync(Examination examination);
    Task DeleteAsync(int id);
}
