using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

namespace LibraryManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<ServiceResult<IEnumerable<Book>>> GetAllBooksAsync()
        {
            try
            {
                var books = await _bookRepository.GetAllAsync();
                return ServiceResult<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<Book>>.Failure($"Ошибка при получении книг: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Book>> GetBookByIdAsync(int id)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(id);
                if (book == null)
                    return ServiceResult<Book>.Failure($"Книга с ID {id} не найдена");

                return ServiceResult<Book>.Success(book);
            }
            catch (Exception ex)
            {
                return ServiceResult<Book>.Failure($"Ошибка при получении книги: {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<Book>>> GetBooksByAuthorAsync(int authorId)
        {
            try
            {
                var authorExists = await _authorRepository.ExistsAsync(authorId);
                if (!authorExists)
                    return ServiceResult<IEnumerable<Book>>.Failure($"Автор с ID {authorId} не найден");

                var books = await _bookRepository.GetByAuthorIdAsync(authorId);
                return ServiceResult<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<Book>>.Failure($"Ошибка при получении книг автора: {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<Book>>> GetBooksPublishedAfterAsync(int year)
        {
            try
            {
                // LINQ запрос через репозиторий
                var books = await _bookRepository.GetBooksPublishedAfterAsync(year);
                return ServiceResult<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<Book>>.Failure($"Ошибка при получении книг: {ex.Message}");
            }
        }

        public async Task<ServiceResult<IEnumerable<Book>>> SearchBooksByTitleAsync(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                    return ServiceResult<IEnumerable<Book>>.Failure("Поисковый запрос не может быть пустым");

                var books = await _bookRepository.SearchByTitleAsync(title);
                return ServiceResult<IEnumerable<Book>>.Success(books);
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<Book>>.Failure($"Ошибка при поиске книг: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Book>> CreateBookAsync(Book book)
        {
            try
            {
                var validationResult = await ValidateBookAsync(book);
                if (!validationResult.IsValid)
                    return ServiceResult<Book>.Failure(validationResult.ErrorMessage);

                var createdBook = await _bookRepository.CreateAsync(book);
                return ServiceResult<Book>.Success(createdBook);
            }
            catch (Exception ex)
            {
                return ServiceResult<Book>.Failure($"Ошибка при создании книги: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Book>> UpdateBookAsync(int id, Book book)
        {
            try
            {
                if (id != book.Id)
                    return ServiceResult<Book>.Failure("ID в URL не совпадает с ID в теле запроса");

                var validationResult = await ValidateBookAsync(book);
                if (!validationResult.IsValid)
                    return ServiceResult<Book>.Failure(validationResult.ErrorMessage);

                var updatedBook = await _bookRepository.UpdateAsync(book);
                if (updatedBook == null)
                    return ServiceResult<Book>.Failure($"Книга с ID {id} не найдена");

                return ServiceResult<Book>.Success(updatedBook);
            }
            catch (Exception ex)
            {
                return ServiceResult<Book>.Failure($"Ошибка при обновлении книги: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteBookAsync(int id)
        {
            try
            {
                var deleted = await _bookRepository.DeleteAsync(id);
                if (!deleted)
                    return ServiceResult<bool>.Failure($"Книга с ID {id} не найдена");

                return ServiceResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure($"Ошибка при удалении книги: {ex.Message}");
            }
        }

        private async Task<(bool IsValid, string ErrorMessage)> ValidateBookAsync(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                return (false, "Название книги не может быть пустым");

            if (book.Title.Length > 200)
                return (false, "Название книги не может превышать 200 символов");

            int currentYear = DateTime.Now.Year;
            if (book.PublishedYear < 1000)
                return (false, "Год публикации не может быть раньше 1000 года");

            if (book.PublishedYear > currentYear + 1)
                return (false, $"Год публикации не может быть больше {currentYear + 1}");

            var author = await _authorRepository.GetByIdAsync(book.AuthorId);
            if (author == null)
                return (false, $"Автор с ID {book.AuthorId} не найден в системе");

            if (author.DateOfBirth.HasValue && book.PublishedYear < author.DateOfBirth.Value.Year)
                return (false, $"Книга не может быть опубликована до года рождения автора ({author.DateOfBirth.Value.Year})");

            return (true, string.Empty);
        }
    }
}