using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services;

public interface IAuthorService //cip...45
{
  Task<Response<List<AuthorReadOnlyDto>>> GetAuthorsAsync();
}