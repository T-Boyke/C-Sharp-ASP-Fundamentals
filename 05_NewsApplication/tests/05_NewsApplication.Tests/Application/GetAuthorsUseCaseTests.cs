using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using _05_NewsApplication.Application.UseCases;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.Interfaces;

namespace _05_NewsApplication.Tests.Application {
    public class GetAuthorsUseCaseTests {
        private class MockAuthorRepo : IAuthorRepository {
            public Task<IEnumerable<Author>> GetAuthorsAsync() {
                var list = new List<Author> {
                    new Author { Id = 1, Firstname = "John", Lastname = "Doe" },
                    new Author { Id = 2, Firstname = "Jane", Lastname = "Doe" }
                };
                return Task.FromResult<IEnumerable<Author>>(list);
            }
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsAllAuthors() {
            // Arrange
            var repo = new MockAuthorRepo();
            var useCase = new GetAuthorsUseCase(repo);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
