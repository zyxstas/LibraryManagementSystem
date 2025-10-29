using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Author>> FindByNameAsync(string name)
        {
            return await _context.Authors
                .Where(a => a.Name.Contains(name))
                .Include(a => a.Books)
                .ToListAsync();
        }

        public async Task<IEnumerable<Author>> FindByNameStartsWithAsync(string prefix)
        {
            return await _context.Authors
                .Where(a => a.Name.StartsWith(prefix))
                .Include(a => a.Books)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAuthorsWithBookCountAsync()
        {
            return await _context.Authors
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.DateOfBirth,
                    BookCount = a.Books.Count
                })
                .ToListAsync();
        }

        public async Task<Author> CreateAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author?> UpdateAsync(Author author)
        {
            var existingAuthor = await _context.Authors.FindAsync(author.Id);
            if (existingAuthor == null) return null;

            existingAuthor.Name = author.Name;
            existingAuthor.DateOfBirth = author.DateOfBirth;

            await _context.SaveChangesAsync();
            return existingAuthor;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Authors.AnyAsync(a => a.Id == id);
        }
    }
}