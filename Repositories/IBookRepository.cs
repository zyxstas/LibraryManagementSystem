using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetByAuthorIdAsync(int authorId);
        Task<IEnumerable<Book>> GetBooksPublishedAfterAsync(int year);
        Task<IEnumerable<Book>> SearchByTitleAsync(string title);
        Task<Book> CreateAsync(Book book);
        Task<Book?> UpdateAsync(Book book);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}