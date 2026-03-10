using _08_Patienten2.Domain.Entities;

namespace _08_Patienten2.Domain.Interfaces;

/// <summary>
/// Interface für spezifische Patienten-Operationen.
/// </summary>
public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<Patient?> GetPatientWithDetailsAsync(int id);
}
