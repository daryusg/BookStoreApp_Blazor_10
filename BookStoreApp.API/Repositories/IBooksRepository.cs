using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Repositories
{
  public interface IBooksRepository : IGenericRepository<Book> //cip...65
  {
    Task<BookDetailsDto?> GetBookDetailsAsync(int id);
    Task<List<BookReadOnlyDto>> GetBooksReadOnlyAsync();
  }
}