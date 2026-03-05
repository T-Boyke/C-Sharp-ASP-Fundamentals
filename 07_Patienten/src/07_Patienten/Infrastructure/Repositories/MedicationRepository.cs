using Microsoft.EntityFrameworkCore;
using _07_Patienten.Domain.Entities;
using _07_Patienten.Domain.Interfaces;
using _07_Patienten.Infrastructure.Data;

namespace _07_Patienten.Infrastructure.Repositories;

/// <summary>
/// Asynchrone Implementierung des Medikamenten-Repositories.
/// </summary>
public class MedicationRepository : IMedicationRepository
{
    private readonly AppDbContext _context;

    public MedicationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Medication>> GetByPatientIdAsync(int patientId)
    {
        return await _context.Medications
            .Where(m => m.PatientId == patientId)
            .OrderByDescending(m => m.PrescribedDate)
            .ToListAsync();
    }

    public async Task AddAsync(Medication medication)
    {
        await _context.Medications.AddAsync(medication);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var medication = await _context.Medications.FindAsync(id);
        if (medication != null)
        {
            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();
        }
    }
}
