using _06_NewsApplication2.Domain.Entities;

namespace _06_NewsApplication2.Domain.Interfaces;

/// <summary>
/// Schnittstelle für das Author-Repository (DDD - Domain Interface).
/// </summary>
public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);
    Task AddAsync(Author author);
    Task UpdateAsync(Author author);
    Task DeleteAsync(int id);
}
