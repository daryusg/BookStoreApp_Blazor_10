using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services;

public interface IAuthorService //cip...45
{
  Task<Response<List<AuthorReadOnlyDto>>> GetAuthorsAsync();
  Task<Response<int>> CreateAuthorAsync(AuthorCreateDto author); //cip...46
  Task<Response<AuthorDetailsDto>> GetAuthorAsync(int id); //cip...47,48
  Task<Response<int>> UpdateAuthorAsync(int id, AuthorUpdateDto author); //cip...47
}