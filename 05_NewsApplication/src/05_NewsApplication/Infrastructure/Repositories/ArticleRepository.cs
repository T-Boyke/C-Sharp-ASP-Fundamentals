using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.Interfaces;
using _05_NewsApplication.Infrastructure.Database;

namespace _05_NewsApplication.Infrastructure.Repositories {
    public class ArticleRepository : IArticleRepository {
        private readonly AppDbContext _context;

        public ArticleRepository(AppDbContext context) {
            _context = context;
        }

        public async Task AddArticleAsync(Article article) {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync() {
            return await _context.Articles
                .Include(a => a.Author)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}
