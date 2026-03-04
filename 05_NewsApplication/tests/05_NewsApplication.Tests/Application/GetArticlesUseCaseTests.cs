using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using _05_NewsApplication.Application.UseCases;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.Interfaces;
using _05_NewsApplication.Domain.ValueObjects;

namespace _05_NewsApplication.Tests.Application {
    public class GetArticlesUseCaseTests {
        private class MockArticleRepo : IArticleRepository {
            public Task AddArticleAsync(Article article) {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<Article>> GetArticlesAsync() {
                var list = new List<Article> {
                    new Article { Id = 1, Headline = new Headline("A1"), Content = "C1", AuthorId = 1, CreatedAt = DateTime.Now.AddDays(-1) },
                    new Article { Id = 2, Headline = new Headline("A2"), Content = "C2", AuthorId = 1, CreatedAt = DateTime.Now }
                };
                return Task.FromResult<IEnumerable<Article>>(list);
            }
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsAllArticles() {
            // Arrange
            var repo = new MockArticleRepo();
            var useCase = new GetArticlesUseCase(repo);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
