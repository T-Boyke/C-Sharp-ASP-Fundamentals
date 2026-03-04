using System.Collections.Generic;
using System.Threading.Tasks;
using _05_NewsApplication.Domain.Entities;
using _05_NewsApplication.Domain.Interfaces;

namespace _05_NewsApplication.Application.UseCases {
    public class GetAuthorsUseCase {
        private readonly IAuthorRepository _repository;

        public GetAuthorsUseCase(IAuthorRepository repository) {
            _repository = repository;
        }

        public async Task<IEnumerable<Author>> ExecuteAsync() {
            return await _repository.GetAuthorsAsync();
        }
    }
}
