using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories
{
  public class AuthorsRepository : GenericRepository<Author>, IAuthorsRepository //cip...65
  {
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public AuthorsRepository(BookStoreDbContext context, IMapper mapper) : base(context)
    {
      this._context = context;
      _mapper = mapper;
    }

    public async Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id)
    {
      var authorDto = await _context.Authors
        .Include(a => a.Books)
        .ProjectTo<AuthorDetailsDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(a => a.Id == id);

      return authorDto;
    }
  }
}
