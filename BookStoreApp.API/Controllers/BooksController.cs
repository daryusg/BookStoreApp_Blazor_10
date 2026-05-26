using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class BooksController : ControllerBase //cip...24
  {
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment; //cip...56

    public BooksController(BookStoreDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
      _context = context;
      this._mapper = mapper;
      this._webHostEnvironment = webHostEnvironment;
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
        .FirstOrDefaultAsync(q => q.Id == id);

      if (bookDto == null)
      {
        return NotFound();
      }

      return bookDto;
    }

    // PUT: api/Books/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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

      //cip...58 if the bookDto contains new image data, i need to store the new image and delete the old one (if exists)
      var picName = Path.GetFileName(book.Image); //store the file name from the old image URL for deletion after the new image is stored successfully
      if (!string.IsNullOrEmpty(bookDto.ImageData) && !string.IsNullOrEmpty(bookDto.OriginalImageName))
        bookDto.Image = await CreateFile(bookDto.ImageData, bookDto.OriginalImageName); //store the new image and get the URL to save in the database

      _mapper.Map(bookDto, book); // Map the updated values from bookDto to the existing book entity
      _context.Entry(book).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();

        if (!string.IsNullOrEmpty(picName))
        {
          //new image was stored successfully SO deleting the old one
          var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "bookcovers", picName); //remove the old image
          if (System.IO.File.Exists(oldImagePath))
          {
            System.IO.File.Delete(oldImagePath);
          }
        }
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
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(StatusCodes.Status201Created)] //CreatedAtAction(string? actionName, object? routeValues, object? value):::Creates a CreatedAtActionResult object that produces a Status201Created response.
    public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto) //cip...25
    {
      var book = _mapper.Map<Book>(bookDto); // Map the BookCreateDto to a Book entity

      if (bookDto.ImageData != null && bookDto.OriginalImageName != null) //cip...56 store the image and get the URL to save in the database
      {
        var imageUrl = await CreateFile(bookDto.ImageData, bookDto.OriginalImageName);
        book.Image = imageUrl;
      }
      _context.Books.Add(book);

      await _context.SaveChangesAsync();

      return CreatedAtAction("GetBook", new { id = book.Id }, book);
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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

    private async Task<string> CreateFile(string imageBase64, string imageName) //cip...56
    {
      var url = HttpContext.Request.Host.Value; // Get the host URL from the current HTTP context
      var ext = Path.GetExtension(imageName); // Get the file extension from the original image name
      var fileName = $"{Guid.NewGuid()}{ext}"; // Generate a unique file name

      var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", "bookcovers", fileName); // Combine the web root path, "images" folder, "bookcovers" folder, and the file name to get the full path
      var bytes = Convert.FromBase64String(imageBase64);
      await using var stream = new MemoryStream(bytes);
      using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
      await stream.CopyToAsync(fileStream);
      return $"https://{url}/images/bookcovers/{fileName}";
    }
  }
}
