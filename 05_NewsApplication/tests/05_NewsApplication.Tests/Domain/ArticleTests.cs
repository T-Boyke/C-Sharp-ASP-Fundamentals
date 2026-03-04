using System;
using Xunit;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.ValueObjects;

namespace _05_NewsApplication.Tests.Domain {
    public class ArticleTests {
        [Fact]
        public void Create_ValidArticle_PropertiesAreSetCorrectly() {
            // Arrange
            var headline = new Headline("My News Headline");
            var content = "This is the content of the news article.";
            var authorId = 42;
            var now = DateTime.UtcNow;

            // Act
            var article = new Article {
                Id = 1,
                Headline = headline,
                Content = content,
                AuthorId = authorId,
                CreatedAt = now
            };

            // Assert
            Assert.Equal(1, article.Id);
            Assert.Equal(headline, article.Headline);
            Assert.Equal(content, article.Content);
            Assert.Equal(authorId, article.AuthorId);
            Assert.Equal(now, article.CreatedAt);
        }
    }
}
