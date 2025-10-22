using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly DataService _dataService;

        public BooksController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            return Ok(_dataService.Books);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _dataService.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound($"Книга с ID {id} не найдена");
            }

            return Ok(book);
        }

        [HttpGet("by-author/{authorId}")]
        public IActionResult GetBooksByAuthor(int authorId)
        {
            var author = _dataService.Authors.FirstOrDefault(a => a.Id == authorId);
            if (author == null)
            {
                return NotFound($"Автор с ID {authorId} не найден");
            }

            var books = _dataService.Books.Where(b => b.AuthorId == authorId).ToList();
            return Ok(books);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
            {
                return BadRequest("Название книги не может быть пустым");
            }

            var author = _dataService.Authors.FirstOrDefault(a => a.Id == book.AuthorId);
            if (author == null)
            {
                return BadRequest($"Автор с ID {book.AuthorId} не существует");
            }

            if (book.PublishedYear < 1000 || book.PublishedYear > DateTime.Now.Year)
            {
                return BadRequest("Год публикации не может быть раньше 1000 года");
            }

            book.Id = _dataService.GetNextBookId();
            _dataService.Books.Add(book);

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
        {
            var existingBook = _dataService.Books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
            {
                return NotFound($"Книга с ID {id} не найдена");
            }

            if (string.IsNullOrWhiteSpace(updatedBook.Title))
            {
                return BadRequest("Название книги не может быть пустым");
            }

            var author = _dataService.Authors.FirstOrDefault(a => a.Id == updatedBook.AuthorId);
            if (author == null)
            {
                return BadRequest($"Автор с ID {updatedBook.AuthorId} не существует");
            }

            if (updatedBook.PublishedYear < 1000 || updatedBook.PublishedYear > DateTime.Now.Year)
            {
                return BadRequest("Год публикации не может быть раньше 1000 года");
            }

            existingBook.Title = updatedBook.Title;
            existingBook.PublishedYear = updatedBook.PublishedYear;
            existingBook.AuthorId = updatedBook.AuthorId;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _dataService.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound($"Книга с ID {id} не найдена");
            }

            _dataService.Books.Remove(book);
            return NoContent();
        }
    }
}