using _06_NewsApplication2.Domain.Entities;

namespace _06_NewsApplication2.Tests.Domain;

public class ArticleTests
{
    [Fact]
    public void ContentPreview_ShouldShortenContent_WhenLongerThan100Chars()
    {
        // Arrange
        var longContent = new string('a', 150);
        var article = new Article 
        { 
            Headline = "Test", 
            Content = longContent,
            AuthorId = 1
        };

        // Act
        var preview = article.ContentPreview;

        // Assert
        Assert.True(preview.Length <= 104); // 100 + "..."
        Assert.EndsWith("...", preview);
    }

    [Fact]
    public void ContentPreview_ShouldNotShorten_WhenShorterThan100Chars()
    {
        // Arrange
        var shortContent = "Short content";
        var article = new Article 
        { 
            Headline = "Test", 
            Content = shortContent,
            AuthorId = 1
        };

        // Act
        var preview = article.ContentPreview;

        // Assert
        Assert.Equal(shortContent, preview);
    }
}
