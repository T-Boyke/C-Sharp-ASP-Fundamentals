using _06_NewsApplication2.Domain.Entities;

namespace _06_NewsApplication2.Domain.Interfaces;

/// <summary>
/// Schnittstelle für das Article-Repository (DDD - Domain Interface).
/// </summary>
public interface IArticleRepository
{
    Task<IEnumerable<Article>> GetAllAsync();
    Task<Article?> GetByIdAsync(int id);
    Task AddAsync(Article article);
    Task UpdateAsync(Article article);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
