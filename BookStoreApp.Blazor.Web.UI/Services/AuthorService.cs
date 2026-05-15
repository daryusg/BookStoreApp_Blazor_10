using Blazored.LocalStorage;
using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services; //cip...45

public class AuthorService : BaseHttpService, IAuthorService
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
        Data = data.ToList()
      };
    }
    catch (ApiException apiException)
    {
      response = ConvertApiExceptions<List<AuthorReadOnlyDto>>(apiException);
    }

    return response;
  }
}
