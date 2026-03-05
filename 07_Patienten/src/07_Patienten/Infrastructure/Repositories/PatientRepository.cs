using Microsoft.EntityFrameworkCore;
using _07_Patienten.Domain.Entities;
using _07_Patienten.Domain.Interfaces;
using _07_Patienten.Infrastructure.Data;

namespace _07_Patienten.Infrastructure.Repositories;

/// <summary>
/// Asynchrone Implementierung des Patient-Repositories.
/// </summary>
public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .Include(p => p.Examinations)
            .Include(p => p.Doctor)
            .Include(p => p.HealthInsurance)
            .Include(p => p.Address)
            .Include(p => p.ContactInfo)
            .OrderBy(p => p.Lastname)
            .ThenBy(p => p.Firstname)
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<Patient?> GetByIdWithExaminationsAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.Examinations)
            .Include(p => p.Medications)
            .Include(p => p.Doctor)
            .Include(p => p.HealthInsurance)
            .Include(p => p.Address)
            .Include(p => p.ContactInfo)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient != null)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}
