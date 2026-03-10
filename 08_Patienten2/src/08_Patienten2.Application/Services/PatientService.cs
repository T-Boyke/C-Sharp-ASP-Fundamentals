using _08_Patienten2.Application.DTOs;
using _08_Patienten2.Application.Interfaces;
using _08_Patienten2.Application.Mapping;
using _08_Patienten2.Domain.Interfaces;

namespace _08_Patienten2.Application.Services;

/// <summary>
/// Implementierung des PatientService.
/// </summary>
public class PatientService(IPatientRepository repository, IUnitOfWork unitOfWork) : IPatientService
{
    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
    {
        var patients = await repository.GetAllAsync();
        return patients.Select(p => p.ToDto());
    }

    public async Task<PatientDto?> GetPatientByIdAsync(int id)
    {
        var patient = await repository.GetPatientWithDetailsAsync(id);
        return patient?.ToDto();
    }

    public async Task<int> CreatePatientAsync(PatientCreateDto createDto)
    {
        var patient = createDto.ToEntity();
        await repository.AddAsync(patient);
        await unitOfWork.SaveChangesAsync();
        return patient.Id;
    }
}
