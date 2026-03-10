using _08_Patienten2.Application.DTOs;

namespace _08_Patienten2.Application.Interfaces;

/// <summary>
/// Schnittstelle für die Geschäftslogik der Patientenverwaltung.
/// </summary>
public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
    Task<PatientDto?> GetPatientByIdAsync(int id);
    Task<int> CreatePatientAsync(PatientCreateDto createDto);
}
