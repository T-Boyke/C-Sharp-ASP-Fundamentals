namespace _08_Patienten2.Domain.Interfaces;

/// <summary>
/// Basis-Interface für generische Repository-Operationen.
/// </summary>
/// <typeparam name="T">Der Typ der Entity.</typeparam>
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
