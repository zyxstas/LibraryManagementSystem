using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public interface IBookService
    {
        Task<ServiceResult<IEnumerable<Book>>> GetAllBooksAsync();
        Task<ServiceResult<Book>> GetBookByIdAsync(int id);
        Task<ServiceResult<IEnumerable<Book>>> GetBooksByAuthorAsync(int authorId);
        Task<ServiceResult<IEnumerable<Book>>> GetBooksPublishedAfterAsync(int year);
        Task<ServiceResult<IEnumerable<Book>>> SearchBooksByTitleAsync(string title);
        Task<ServiceResult<Book>> CreateBookAsync(Book book);
        Task<ServiceResult<Book>> UpdateBookAsync(int id, Book book);
        Task<ServiceResult<bool>> DeleteBookAsync(int id);
    }
}