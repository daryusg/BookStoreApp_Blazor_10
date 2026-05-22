using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services;

public interface IAuthorService //cip...45
{
  Task<Response<List<AuthorReadOnlyDto>>> GetAsync();
  Task<Response<int>> CreateAsync(AuthorCreateDto author); //cip...46
  Task<Response<AuthorDetailsDto>> GetAsync(int id); //cip...47,48
  Task<Response<int>> UpdateAsync(int id, AuthorUpdateDto author); //cip...47
  Task<Response<int>> DeleteAsync(int id); //cip...49
}