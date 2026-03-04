using System.Collections.Generic;
using System.Threading.Tasks;
using _05_NewsApplication.Domain.Entities;

namespace _05_NewsApplication.Domain.Interfaces {
    public interface IAuthorRepository {
        Task<IEnumerable<Author>> GetAuthorsAsync();
    }
}
