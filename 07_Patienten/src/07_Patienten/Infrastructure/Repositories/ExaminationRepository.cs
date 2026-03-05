using Microsoft.EntityFrameworkCore;
using _07_Patienten.Domain.Entities;
using _07_Patienten.Domain.Interfaces;
using _07_Patienten.Infrastructure.Data;

namespace _07_Patienten.Infrastructure.Repositories;

/// <summary>
/// Asynchrone Implementierung des Examination-Repositories.
/// </summary>
public class ExaminationRepository : IExaminationRepository
{
    private readonly AppDbContext _context;

    public ExaminationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Examination>> GetByPatientIdAsync(int patientId)
    {
        return await _context.Examinations
            .Where(e => e.PatientId == patientId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }

    public async Task<Examination?> GetByIdAsync(int id)
    {
        return await _context.Examinations.FindAsync(id);
    }

    public async Task AddAsync(Examination examination)
    {
        await _context.Examinations.AddAsync(examination);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var examination = await _context.Examinations.FindAsync(id);
        if (examination != null)
        {
            _context.Examinations.Remove(examination);
            await _context.SaveChangesAsync();
        }
    }
}
