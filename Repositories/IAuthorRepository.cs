using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<IEnumerable<Author>> FindByNameAsync(string name);
        Task<IEnumerable<Author>> FindByNameStartsWithAsync(string prefix);
        Task<IEnumerable<object>> GetAuthorsWithBookCountAsync();
        Task<Author> CreateAsync(Author author);
        Task<Author?> UpdateAsync(Author author);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}