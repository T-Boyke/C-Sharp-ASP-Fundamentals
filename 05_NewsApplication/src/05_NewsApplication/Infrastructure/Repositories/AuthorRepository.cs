using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.Interfaces;
using _05_NewsApplication.Infrastructure.Database;

namespace _05_NewsApplication.Infrastructure.Repositories {
    public class AuthorRepository : IAuthorRepository {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync() {
            return await _context.Authors.ToListAsync();
        }
    }
}
