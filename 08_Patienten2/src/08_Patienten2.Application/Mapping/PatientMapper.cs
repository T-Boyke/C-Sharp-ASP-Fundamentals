using _08_Patienten2.Application.DTOs;
using _08_Patienten2.Domain.Entities;

namespace _08_Patienten2.Application.Mapping;

/// <summary>
/// Manueller Mapper für Patienten-Objekte (High Performance & IHK Standard).
/// </summary>
public static class PatientMapper
{
    public static PatientDto ToDto(this Patient patient)
    {
        return new PatientDto(
            patient.Id,
            patient.Firstname,
            patient.Lastname,
            $"{patient.Firstname} {patient.Lastname}",
            patient.Birthdate,
            patient.SocialSecurityNumber,
            patient.Age,
            patient.Symptoms
        );
    }

    public static Patient ToEntity(this PatientCreateDto dto)
    {
        return new Patient(
            dto.Firstname,
            dto.Lastname,
            dto.Birthdate,
            dto.SocialSecurityNumber
        );
    }
}
