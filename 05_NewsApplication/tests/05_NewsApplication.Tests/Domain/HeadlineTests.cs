using System;
using Xunit;
using _05_NewsApplication.Domain.ValueObjects;

namespace _05_NewsApplication.Tests.Domain {
    public class HeadlineTests {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_NullOrEmpty_ThrowsArgumentException(string invalidHeadline) {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Headline(invalidHeadline));
            Assert.Contains("Headline cannot be empty", exception.Message);
        }

        [Fact]
        public void Create_TooLong_ThrowsArgumentException() {
            // Arrange
            var longHeadline = new string('a', 201); // Assuming max length 200

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new Headline(longHeadline));
            Assert.Contains("Headline cannot be longer than 200 characters.", exception.Message);
        }

        [Fact]
        public void Create_ValidString_SetsValue() {
            // Arrange
            var validTitle = "Breaking News!";

            // Act
            var headline = new Headline(validTitle);

            // Assert
            Assert.Equal(validTitle, headline.Value);
        }

        [Fact]
        public void Equality_SameValue_AreEqual() {
            // Arrange
            var h1 = new Headline("Same Title");
            var h2 = new Headline("Same Title");

            // Act & Assert
            Assert.Equal(h1, h2);
            // Since it will be a record, == should also work
            Assert.True(h1 == h2);
        }
    }
}
