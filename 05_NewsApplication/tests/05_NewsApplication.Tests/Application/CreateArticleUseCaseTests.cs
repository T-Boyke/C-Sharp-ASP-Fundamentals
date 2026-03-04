using System;
using System.Threading.Tasks;
using Xunit;
using _05_NewsApplication.Application.UseCases;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.Interfaces;
using _05_NewsApplication.Domain.ValueObjects;

namespace _05_NewsApplication.Tests.Application {
    public class CreateArticleUseCaseTests {
        private class MockArticleRepo : IArticleRepository {
            public Article? AddedArticle { get; private set; }

            public Task AddArticleAsync(Article article) {
                AddedArticle = article;
                return Task.CompletedTask;
            }

            public Task<System.Collections.Generic.IEnumerable<Article>> GetArticlesAsync() {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public async Task ExecuteAsync_AddsArticleWithCurrentDate() {
            // Arrange
            var repo = new MockArticleRepo();
            var useCase = new CreateArticleUseCase(repo);
            var headline = new Headline("Test Headline");
            var article = new Article {
                Id = 0,
                Headline = headline,
                Content = "Test content",
                AuthorId = 1
            };

            // Act
            await useCase.ExecuteAsync(article);

            // Assert
            Assert.NotNull(repo.AddedArticle);
            Assert.Equal(headline, repo.AddedArticle.Headline);
            // It should set the CreatedAt
            Assert.NotEqual(default, repo.AddedArticle.CreatedAt);
        }
    }
}
