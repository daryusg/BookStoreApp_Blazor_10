using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;

namespace BookStoreApp.API.Repositories
{
  public interface IAuthorsRepository : IGenericRepository<Author> //cip...65
  {
    Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id); //cip...65
  }
}
