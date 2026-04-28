using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class AuthorsController : ControllerBase //cip...19
  {
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthorsController> _logger;

    //public AuthorsController(BookStoreDbContext context, IMapper mapper)
    public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger) //cip...20, 21
    {
      _context = context;
      this._mapper = mapper;
      this._logger = logger;
    }

    // GET: api/Authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors() //cip...20
    {
      try //cip...21
      {
        _logger.LogInformation($"Request to {nameof(GetAuthors)}"); //cip...21
        //throw new Exception("Test exception in GetAuthors"); //cip...21 *** REMOVE THIS LINE AFTER TESTING ***

        var authors = await _context.Authors.ToListAsync();
        var authorsDto = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
        return Ok(authorsDto);
      }
      catch (Exception ex) //cip...21
      {
        _logger.LogError(ex, $"An error occurred in {nameof(GetAuthors)}");
        return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
      }
    }

    // GET: api/Authors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
    {
      try //cip...21
      {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
          _logger.LogWarning($"Author with id {id} not found in {nameof(GetAuthor)}"); //cip...21
          return NotFound();
        }

        var authorDto = _mapper.Map<AuthorReadOnlyDto>(author); //cip...20
        return Ok(authorDto);
      }
      catch (Exception ex) //cip...21
      {
        _logger.LogError(ex, $"An error occurred in {nameof(GetAuthor)}");
        return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
      }
    }

    // PUT: api/Authors/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto) //cip...20
    {
      try //cip...21
      {
        if (id != authorDto.Id)
        {
          return BadRequest();
        }

        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
          _logger.LogWarning($"Author with id {id} not found in {nameof(PutAuthor)}"); //cip...21
          return NotFound();
        }

        _mapper.Map(authorDto, author); //cip...20
        _context.Entry(author).State = EntityState.Modified;

        try
        {
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!await AuthorExistsAsync(id)) //cip...20
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }

        return NoContent();
      }
      catch (Exception ex) //cip...21
      {
        _logger.LogError(ex, $"An error occurred in {nameof(PutAuthor)}");
        return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
      }
    }

    // POST: api/Authors
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)  //cip...20
    {
      try //cip...21
      {
        /*method 1
        var author = new Author
        {
          FirstName = authorDto.FirstName,
          LastName = authorDto.LastName,
          Bio = authorDto.Bio
        };
        */
        //method2 use automapper
        var author = _mapper.Map<Author>(authorDto); //cip...20
        await _context.Authors.AddAsync(author); //cip...20
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);  //cip...20
      }
      catch (Exception ex) //cip...21
      {
        _logger.LogError(ex, $"An error occurred in {nameof(PostAuthor)}");
        return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
      }
    }

    // DELETE: api/Authors/5
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
      try //cip...21
      {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
          _logger.LogWarning($"Author with id {id} not found in {nameof(DeleteAuthor)}"); //cip...21
          return NotFound();
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
      }
      catch (Exception ex) //cip...21
      {
        _logger.LogError(ex, $"An error occurred in {nameof(DeleteAuthor)}");
        return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
      }
    }

    private async Task<bool> AuthorExistsAsync(int id)
    {
      return await _context.Authors.AnyAsync(e => e.Id == id);
    }
  }
}
