using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly DataService _dataService;

        public AuthorsController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult GetAllAuthors()
        {
            return Ok(_dataService.Authors);
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var author = _dataService.Authors.FirstOrDefault(a => a.Id == id);

            if (author == null)
            {
                return NotFound($"Автор с ID {id} не найден");
            }

            return Ok(author);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] Author author)
        {
            if (string.IsNullOrWhiteSpace(author.Name))
            {
                return BadRequest("Имя автора обязательно");
            }

            author.Id = _dataService.GetNextAuthorId();

            _dataService.Authors.Add(author);

            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] Author updatedAuthor)
        {
            var existingAuthor = _dataService.Authors.FirstOrDefault(a => a.Id == id);

            if (existingAuthor == null)
            {
                return NotFound($"Автор с ID {id} не найден");
            }

            if (string.IsNullOrWhiteSpace(updatedAuthor.Name))
            {
                return BadRequest("Имя автора обязательно");
            }

            existingAuthor.Name = updatedAuthor.Name;
            existingAuthor.DateOfBirth = updatedAuthor.DateOfBirth;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var author = _dataService.Authors.FirstOrDefault(a => a.Id == id);

            if (author == null)
            {
                return NotFound($"Автор с ID {id} не найден");
            }

            var authorBooks = _dataService.Books.Where(b => b.AuthorId == id).ToList();
            if (authorBooks.Any())
            {
                return BadRequest("Нельзя удалить автора, у которого есть книги. Сначала удалите все книги автора.");
            }

            _dataService.Authors.Remove(author);

            return NoContent();
        }
    }
}