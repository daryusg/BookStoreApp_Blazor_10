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

  public async Task<Response<int>> CreateAuthorAsync(AuthorCreateDto author) //cip...46
  {
    Response<int> response = new() { Success = true };
    try
    {
      await GetBearerTokenAsync();
      await _client.AuthorsPOSTAsync(author);
    }
    catch (ApiException apiException)
    {
      response = ConvertApiExceptions<int>(apiException);
    }

    return response;
  }

  public async Task<Response<AuthorReadOnlyDto>> GetAuthorAsync(int id) //cip...47
  {
    Response<AuthorReadOnlyDto> response;
    try
    {
      await GetBearerTokenAsync();
      var data = await _client.AuthorsGETAsync(id);
      response = new Response<AuthorReadOnlyDto>
      {
        Data = data,
        Success = true
      };
    }
    catch (ApiException apiException)
    {
      response = ConvertApiExceptions<AuthorReadOnlyDto>(apiException);
    }

    return response;
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

  public async Task<Response<int>> UpdateAuthorAsync(int id, AuthorUpdateDto author) //cip...47
  {
    Response<int> response = new() { Success = true };
    try
    {
      await GetBearerTokenAsync();
      await _client.AuthorsPUTAsync(id, author);
    }
    catch (ApiException apiException)
    {
      response = ConvertApiExceptions<int>(apiException);
    }

    return response;
  }
}
