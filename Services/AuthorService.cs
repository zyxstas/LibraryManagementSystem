using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<ServiceResult<IEnumerable<Author>>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            return ServiceResult<IEnumerable<Author>>.Success(authors); 
        }

        public async Task<ServiceResult<Author>> GetAuthorByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
                return ServiceResult<Author>.Failure($"Автор с ID {id} не найден");

            return ServiceResult<Author>.Success(author);
        }

        public async Task<ServiceResult<IEnumerable<Author>>> SearchAuthorsAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ServiceResult<IEnumerable<Author>>.Failure("Поисковый запрос не может быть пустым");

            var authors = await _authorRepository.FindByNameAsync(name);
            return ServiceResult<IEnumerable<Author>>.Success(authors);
        }

        public async Task<ServiceResult<IEnumerable<Author>>> GetAuthorsByNameStartsWithAsync(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return ServiceResult<IEnumerable<Author>>.Failure("Префикс не может быть пустым");

            var authors = await _authorRepository.FindByNameStartsWithAsync(prefix);
            return ServiceResult<IEnumerable<Author>>.Success(authors);
        }

        public async Task<ServiceResult<IEnumerable<object>>> GetAuthorsWithBookCountAsync()
        {
            var authors = await _authorRepository.GetAuthorsWithBookCountAsync();
            return ServiceResult<IEnumerable<object>>.Success(authors);
        }

        public async Task<ServiceResult<Author>> CreateAuthorAsync(Author author)
        {
            var validationResult = ValidateAuthor(author);
            if (!validationResult.IsValid)
                return ServiceResult<Author>.Failure(validationResult.ErrorMessage);

            var createdAuthor = await _authorRepository.CreateAsync(author);
            return ServiceResult<Author>.Success(createdAuthor);
        }

        public async Task<ServiceResult<Author>> UpdateAuthorAsync(int id, Author author)
        {
            if (id != author.Id)
                return ServiceResult<Author>.Failure("ID в URL не совпадает с ID в теле запроса");

            var validationResult = ValidateAuthor(author);
            if (!validationResult.IsValid)
                return ServiceResult<Author>.Failure(validationResult.ErrorMessage);

            var updatedAuthor = await _authorRepository.UpdateAsync(author);
            if (updatedAuthor == null)
                return ServiceResult<Author>.Failure($"Автор с ID {id} не найден");

            return ServiceResult<Author>.Success(updatedAuthor);
        }

        public async Task<ServiceResult<bool>> DeleteAuthorAsync(int id)
        {
            var deleted = await _authorRepository.DeleteAsync(id);
            if (!deleted)
                return ServiceResult<bool>.Failure($"Автор с ID {id} не найден");

            return ServiceResult<bool>.Success(true);
        }

        private (bool IsValid, string ErrorMessage) ValidateAuthor(Author author)
        {
            if (string.IsNullOrWhiteSpace(author.Name))
                return (false, "Имя автора не может быть пустым");

            if (author.Name.Length > 100)
                return (false, "Имя автора не может превышать 100 символов");

            if (author.DateOfBirth.HasValue && author.DateOfBirth.Value > DateTime.Now)
                return (false, "Дата рождения не может быть в будущем");

            return (true, string.Empty);
        }
    }
}