using AutoMapper;
//using AutoMapper.QueryableExtensions; cip...65 commented
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;
using BookStoreApp.API.Repositories;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BooksController : ControllerBase //cip...24
{
    private readonly IBooksRepository _booksRepository;

    //private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment; //cip...56
    private readonly ILogger<BooksController> _logger; //cip...20260609 injecting a logger to log any exceptions that might occur during the update process

    //public BooksController(BookStoreDbContext _context, IMapper _mapper, IWebHostEnvironment webHostEnvironment)
    public BooksController(IBooksRepository booksRepository, IMapper mapper, IWebHostEnvironment webHostEnvironment, ILogger<BooksController> logger)
    {
        this._booksRepository = booksRepository;
        //_context = _context;
        this._mapper = mapper;
        this._webHostEnvironment = webHostEnvironment;
        this._logger = logger;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks() //cip...25
    {
        try
        {
            //var books = await _authorsRepository.Books
            //var booksDto = await _context.Books
            //  .Include(b => b.Author) // Include = LEFT (OUTER?) JOIN
            //  .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider) //use AutoMapper to project directly to BookReadOnlyDto
            //  .ToListAsync();
            //var booksDto = _mapper.Map<IEnumerable<BookReadOnlyDto>>(books);
            var booksDto = await _booksRepository.GetBooksReadOnlyAsync(); //cip...24
            return Ok(booksDto);
        }
        catch (Exception ex) //cip...20260609 catch any exception that might occur during the retrieval process and log it, then return a generic error message to the client
        {
            _logger.LogError(ex, $"An error occurred in {nameof(GetBooks)}");
            return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
        }
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
    {
        try
        {
            //var bookDto = await _context.Books
            //  .Include(b => b.Author)
            //  .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
            //  .FirstOrDefaultAsync(q => q.Id == id);
            var bookDto = await _booksRepository.GetBookDetailsAsync(id); //cip...65

            if (bookDto == null)
            {
                return NotFound();
            }

            return bookDto;
        }
        catch (Exception ex) //cip...20260609 catch any exception that might occur during the retrieval process and log it, then return a generic error message to the client
        {
            _logger.LogError(ex, $"An error occurred in {nameof(GetBook)}");
            return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
        }
    }

    // PUT: api/Books/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto) //cip...25
    {
        _logger.LogInformation("PutBook entered. Id={Id}, Title={Title}, ImageDataLength={Length}", id, bookDto.Title, bookDto.ImageData?.Length ?? 0);
        if (id != bookDto.Id)
        {
            return BadRequest();
        }

        //var book = await _context.Books.FindAsync(id);
        var book = await _booksRepository.GetAsync(id); //cip...65
        if (book == null)
        {
            return NotFound();
        }

        try
        {
            //cip...58 if the bookDto contains new image data, i need to store the new image and delete the old one (if exists)
            var picName = Path.GetFileName(book.Image); //store the file name from the old image URL for deletion after the new image is stored successfully
            if (!string.IsNullOrEmpty(bookDto.ImageData) && !string.IsNullOrEmpty(bookDto.OriginalImageName))
                bookDto.Image = await CreateFile(bookDto.ImageData, bookDto.OriginalImageName); //store the new image and get the URL to save in the database

            _mapper.Map(bookDto, book); // Map the updated values from bookDto to the existing book entity
                                        //_context.Entry(book).State = EntityState.Modified;

            //await _context.SaveChangesAsync();
            await _booksRepository.UpdateAsync(book); //cip...65

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
        catch (Exception ex) //cip...20260609 catch any exception that might occur during the update process and log it, then return a generic error message to the client
        {
            _logger.LogError(ex, $"Error in {nameof(PutBook)} updating book: Id: {id}. Title: {book.Title}.");

            return StatusCode(500, $"An error occurred while updating the book: {book.Title}.");
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
        try
        {
            _logger.LogInformation("PostBook entered.");
            var book = _mapper.Map<Book>(bookDto); // Map the BookCreateDto to a Book entity

            if (bookDto.ImageData != null && bookDto.OriginalImageName != null) //cip...56 store the image and get the URL to save in the database
            {
                var imageUrl = await CreateFile(bookDto.ImageData, bookDto.OriginalImageName);
                book.Image = imageUrl;
            }
            //_context.Books.Add(book);

            //await _context.SaveChangesAsync();
            await _booksRepository.AddAsync(book); //cip...65

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }
        catch (Exception ex) //cip...20260609 catch any exception that might occur during the create process and log it, then return a generic error message to the client
        {
            _logger.LogError(ex, $"Error occured in {nameof(PostBook)} creating book: {bookDto.Title}.");

            return StatusCode(500, $"An error occurred while creating the book: {bookDto.Title}.");
        }
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Administrator)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBook(int id)
    {
        try
        {
            //var book = await _context.Books.FindAsync(id);
            var book = await _booksRepository.GetAsync(id); //cip...65
            if (book == null)
            {
                return NotFound();
            }

            //_context.Books.Remove(book);
            //await _context.SaveChangesAsync();
            await _booksRepository.DeleteAsync(id); //cip...65

            return NoContent();
        }
        catch (Exception ex) //cip...20260609 catch any exception that might occur during the process and log it, then return a generic error message to the client
        {
            _logger.LogError(ex, $"An error occurred in {nameof(DeleteBook)}");
            return StatusCode(500, Messages.Error500Message); // Return a 500 Internal Server Error response
        }
    }

    private async Task<bool> BookExistsAsync(int id) //cip...25
    {
        //return await _context.Books.AnyAsync(e => e.Id == id);
        return await _booksRepository.Exists(id);
    }

    private async Task<string> CreateFile(string imageBase64, string imageName) //cip...56, 20260609 chatgpt
    {
        try
        {
            var url = HttpContext.Request.Host.Value; // Get the host URL from the current HTTP _context
            var ext = Path.GetExtension(imageName); // Get the file extension from the original image name
            var fileName = $"{Guid.NewGuid()}{ext}"; // Generate a unique file name

            var folder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "bookcovers");
            Directory.CreateDirectory(folder); // Ensure the directory exists

            var path = Path.Combine(folder, fileName); // Combine the web root path, "images" folder, "bookcovers" folder, and the file name to get the full path

            _logger.LogInformation(
                "Creating image file. WebRootPath: {WebRootPath}, Folder: {Folder}, Path: {Path}, ImageName: {ImageName}",
                _webHostEnvironment.WebRootPath,
                folder,
                path,
                imageName);

            var bytes = Convert.FromBase64String(imageBase64);
            await using var stream = new MemoryStream(bytes);
            await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);

            return $"https://{url}/images/bookcovers/{fileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "CreateFile failed in {MethodName}. WebRootPath: {WebRootPath}, ImageName: {ImageName}, ImageDataLength: {Length}",
                nameof(CreateFile),
                _webHostEnvironment.WebRootPath,
                imageName,
                imageBase64?.Length ?? 0);

            throw;
        }
    }

    [HttpGet("testwrite")]
    [AllowAnonymous] // temporary
    public IActionResult TestWrite()
    {
        try
        {
            var folder = Path.Combine(
                _webHostEnvironment.WebRootPath,
                "images",
                "bookcovers");

            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, "test.txt");

            System.IO.File.WriteAllText(
                filePath,
                $"Test file created at {DateTime.UtcNow}");

            _logger.LogCritical(
                "TESTWRITE succeeded. File created at {FilePath}",
                filePath);

            return Ok(new
            {
                Success = true,
                Folder = folder,
                FilePath = filePath
            });
        }
        catch (Exception ex)
        {
            _logger.LogCritical(
                ex,
                "TESTWRITE failed");

            return StatusCode(500, ex.ToString());
        }
    }
}
