using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public interface IAuthorService
    {
        Task<ServiceResult<IEnumerable<Author>>> GetAllAuthorsAsync();
        Task<ServiceResult<Author>> GetAuthorByIdAsync(int id);
        Task<ServiceResult<IEnumerable<Author>>> SearchAuthorsAsync(string name);
        Task<ServiceResult<IEnumerable<Author>>> GetAuthorsByNameStartsWithAsync(string prefix);
        Task<ServiceResult<IEnumerable<object>>> GetAuthorsWithBookCountAsync();
        Task<ServiceResult<Author>> CreateAuthorAsync(Author author);
        Task<ServiceResult<Author>> UpdateAuthorAsync(int id, Author author);
        Task<ServiceResult<bool>> DeleteAuthorAsync(int id);
    }
}