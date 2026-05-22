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

  public async Task<Response<int>> CreateAsync(AuthorCreateDto author) //cip...46
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

  public async Task<Response<int>> DeleteAsync(int id) //cip...49
  {
    Response<int> response = new() { Success = true };
    try
    {
      await GetBearerTokenAsync();
      await _client.AuthorsDELETEAsync(id);
    }
    catch (ApiException apiException)
    {
      response = ConvertApiExceptions<int>(apiException);
    }

    return response;
  }

  public async Task<Response<AuthorDetailsDto>> GetAsync(int id) //cip...47,48
  {
    Response<AuthorDetailsDto> response;
    try
    {
      await GetBearerTokenAsync();
      var data = await _client.AuthorsGETAsync(id);
      response = new Response<AuthorDetailsDto>
      {
        Data = data,
        Success = true
      };
    }
    catch (ApiException apiException)
    {
      response = ConvertApiExceptions<AuthorDetailsDto>(apiException);
    }

    return response;
  }

  public async Task<Response<List<AuthorReadOnlyDto>>> GetAsync()
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

  public async Task<Response<int>> UpdateAsync(int id, AuthorUpdateDto author) //cip...47
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
