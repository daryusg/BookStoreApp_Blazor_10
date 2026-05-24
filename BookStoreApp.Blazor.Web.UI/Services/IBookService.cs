using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services;

public interface IBookService //cip...53
{
  Task<Response<List<BookReadOnlyDto>>> GetAsync();
  Task<Response<int>> CreateAsync(BookCreateDto Book);
  Task<Response<BookDetailsDto>> GetAsync(int id);
  Task<Response<int>> UpdateAsync(int id, BookUpdateDto Book);
  Task<Response<int>> DeleteAsync(int id);
}
