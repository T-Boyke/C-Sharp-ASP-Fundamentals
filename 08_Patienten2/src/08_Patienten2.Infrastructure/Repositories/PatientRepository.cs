using _08_Patienten2.Domain.Entities;
using _08_Patienten2.Domain.Interfaces;
using _08_Patienten2.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace _08_Patienten2.Infrastructure.Repositories;

/// <summary>
/// Repository-Implementierung für Patienten-Daten.
/// </summary>
public class PatientRepository(ApplicationDbContext context) : IPatientRepository
{
    public async Task<Patient?> GetByIdAsync(int id) => await context.Patients.FindAsync(id);

    public async Task<IEnumerable<Patient>> GetAllAsync() => await context.Patients.ToListAsync();

    public async Task AddAsync(Patient entity) => await context.Patients.AddAsync(entity);

    public void Update(Patient entity) => context.Patients.Update(entity);

    public void Delete(Patient entity) => context.Patients.Remove(entity);

    public async Task<Patient?> GetPatientWithDetailsAsync(int id)
    {
        return await context.Patients
            .Include(p => p.Examinations)
            .Include(p => p.Doctor)
            .Include(p => p.HealthInsurance)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
}
