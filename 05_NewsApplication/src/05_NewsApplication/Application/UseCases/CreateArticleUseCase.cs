using System;
using System.Threading.Tasks;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.Interfaces;

namespace _05_NewsApplication.Application.UseCases {
    public class CreateArticleUseCase {
        private readonly IArticleRepository _repository;

        public CreateArticleUseCase(IArticleRepository repository) {
            _repository = repository;
        }

        public async Task ExecuteAsync(Article article) {
            article.CreatedAt = DateTime.UtcNow; // Or Now depending on requirement
            await _repository.AddArticleAsync(article);
        }
    }
}
