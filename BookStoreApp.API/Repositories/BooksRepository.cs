using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories
{
  public class BooksRepository : GenericRepository<Book>, IBooksRepository //cip...65
  {
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public BooksRepository(BookStoreDbContext context, IMapper mapper) : base(context)
    {
      this._context = context;
      this._mapper = mapper;
    }

    public async Task<BookDetailsDto?> GetBookDetailsAsync(int id)
    {
      var bookDto = await _context.Books
        .Include(b => b.Author)
        .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(q => q.Id == id);

      return bookDto;
    }

    public async Task<List<BookReadOnlyDto>> GetBooksReadOnlyAsync()
    {
      var booksDto = await _context.Books
        .Include(b => b.Author) // Include = LEFT (OUTER?) JOIN
        .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider) //use AutoMapper to project directly to BookReadOnlyDto
        .ToListAsync();

      return booksDto;
    }

  }
}