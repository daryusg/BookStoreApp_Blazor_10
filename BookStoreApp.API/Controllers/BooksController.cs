using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BooksController : ControllerBase //cip...24
  {
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public BooksController(BookStoreDbContext context, IMapper mapper)
    {
      _context = context;
      this._mapper = mapper;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks() //cip...25
    {
      //var books = await _context.Books
      var booksDto = await _context.Books
        .Include(b => b.Author) // Include = LEFT (OUTER?) JOIN
        .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider) //use AutoMapper to project directly to BookReadOnlyDto
        .ToListAsync();
      //var booksDto = _mapper.Map<IEnumerable<BookReadOnlyDto>>(books);
      return Ok(booksDto);
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
    {
      var bookDto = await _context.Books
        .Include(b => b.Author)
        .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(q => q.Id ==id);

      if (bookDto == null)
      {
        return NotFound();
      }

      return bookDto;
    }

    // PUT: api/Books/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto) //cip...25
    {
      if (id != bookDto.Id)
      {
        return BadRequest();
      }

      var book = await _context.Books.FindAsync(id);
      if (book == null)
      {
        return NotFound();
      }

      _mapper.Map(bookDto, book); // Map the updated values from bookDto to the existing book entity
      _context.Entry(book).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!await BookExistsAsync(id))
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

    // POST: api/Books
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto) //cip...25
    {
      var book = _mapper.Map<Book>(bookDto); // Map the BookCreateDto to a Book entity
      _context.Books.Add(book);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetBook", new { id = book.Id }, book);
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
      var book = await _context.Books.FindAsync(id);
      if (book == null)
      {
        return NotFound();
      }

      _context.Books.Remove(book);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private async Task<bool> BookExistsAsync(int id) //cip...25
    {
      return await _context.Books.AnyAsync(e => e.Id == id);
    }
  }
}
