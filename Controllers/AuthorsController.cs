using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAllAuthors()
        {
            var result = await _authorService.GetAllAuthorsAsync();
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthorById(int id)
        {
            var result = await _authorService.GetAuthorByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Author>>> SearchAuthors([FromQuery] string name)
        {
            var result = await _authorService.SearchAuthorsAsync(name);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("name-starts-with")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthorsByNameStartsWith([FromQuery] string prefix)
        {
            var result = await _authorService.GetAuthorsByNameStartsWithAsync(prefix);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpGet("with-book-count")]
        public async Task<ActionResult<IEnumerable<object>>> GetAuthorsWithBookCount()
        {
            var result = await _authorService.GetAuthorsWithBookCountAsync();
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor([FromBody] Author author)
        {
            var result = await _authorService.CreateAuthorAsync(author);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetAuthorById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
        {
            var result = await _authorService.UpdateAuthorAsync(id, author);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var result = await _authorService.DeleteAuthorAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            return NoContent();
        }
    }
}