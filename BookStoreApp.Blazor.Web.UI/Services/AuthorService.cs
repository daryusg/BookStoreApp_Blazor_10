using Blazored.LocalStorage;
using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services;

public class AuthorService : BaseHttpService, IAuthorService //cip...45
{
  private readonly IClient _client;

  public AuthorService(IClient client, ILocalStorageService localStorageService) : base(client, localStorageService)
  {
    this._client = client;
  }

  public async Task<Response<List<AuthorReadOnlyDto>>> GetAuthorsAsync()
  {
    Response<List<AuthorReadOnlyDto>> response;
    try
    {
      await GetBearerTokenAsync();
      var data = await _client.AuthorsAllAsync();
      response = new Response<List<AuthorReadOnlyDto>>
      {
        Data = data.ToList(),
        Success = true
      };
    }
    catch (ApiException apiException)
    {
      response = ConvertApiExceptions<List<AuthorReadOnlyDto>>(apiException);
    }

    return response;
  }
}
