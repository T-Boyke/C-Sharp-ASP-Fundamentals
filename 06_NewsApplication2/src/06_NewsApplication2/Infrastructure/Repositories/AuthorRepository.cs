using Microsoft.EntityFrameworkCore;
using _06_NewsApplication2.Domain.Entities;
using _06_NewsApplication2.Domain.Interfaces;
using _06_NewsApplication2.Infrastructure.Data;

namespace _06_NewsApplication2.Infrastructure.Repositories;

/// <summary>
/// Asynchrone Implementierung des Author-Repositories.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _context;

    public AuthorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _context.Authors
            .OrderBy(a => a.Firstname)
            .ThenBy(a => a.Lastname)
            .ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task AddAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var author = await GetByIdAsync(id);
        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
}
