using Microsoft.EntityFrameworkCore;
using _06_NewsApplication2.Domain.Entities;
using _06_NewsApplication2.Domain.Interfaces;
using _06_NewsApplication2.Infrastructure.Data;

namespace _06_NewsApplication2.Infrastructure.Repositories;

/// <summary>
/// Asynchrone Implementierung des Article-Repositories.
/// </summary>
public class ArticleRepository : IArticleRepository
{
    private readonly AppDbContext _context;

    public ArticleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await _context.Articles
            .Include(a => a.Author)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _context.Articles
            .Include(a => a.Author)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Article article)
    {
        await _context.Articles.AddAsync(article);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Article article)
    {
        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var article = await GetByIdAsync(id);
        if (article != null)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Articles.AnyAsync(e => e.Id == id);
    }
}
