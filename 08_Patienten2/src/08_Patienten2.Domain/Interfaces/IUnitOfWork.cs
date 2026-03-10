namespace _08_Patienten2.Domain.Interfaces;

/// <summary>
/// Interface für das Unit of Work Pattern zur Sicherstellung der Atomarität.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
