using System.Collections.Generic;
using System.Threading.Tasks;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.Interfaces;

namespace _05_NewsApplication.Application.UseCases {
    public class GetArticlesUseCase {
        private readonly IArticleRepository _repository;

        public GetArticlesUseCase(IArticleRepository repository) {
            _repository = repository;
        }

        public async Task<IEnumerable<Article>> ExecuteAsync() {
            return await _repository.GetArticlesAsync();
        }
    }
}
