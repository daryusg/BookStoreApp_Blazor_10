using Blazored.LocalStorage;
using BookStoreApp.Blazor.Web.UI.Models;
using BookStoreApp.Blazor.Web.UI.Services.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Web.UI.Services;

public class AuthorService : BaseHttpService, IAuthorService //cip...45
{
  private readonly IClient _client;

  //public AuthorService(IClient client, ILocalStorageService localStorageService) : base(client, localStorageService)
  public AuthorService(IClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider,
    NavigationManager navigationManager) : base(client, localStorageService, authenticationStateProvider, navigationManager) //cip...50 chatgpt fix: add authentication state provider and navigation manager to constructor to log the user out if the token's expired.
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
      response = await ConvertApiExceptions<int>(apiException);
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
      response = await ConvertApiExceptions<int>(apiException);
    }

    return response;
  }

  public async Task<Response<AuthorDetailsDto>> GetAsync(int id) //cip...47,48
  {
    Response<AuthorDetailsDto> response;
    try
    {
      await GetBearerTokenAsync();
      //var data = await _client.AuthorsGETAsync(id);
      var data = await _client.AuthorsGET2Async(id); //cip...66
      response = new Response<AuthorDetailsDto>
      {
        Data = data,
        Success = true
      };
    }
    catch (ApiException apiException)
    {
      response = await ConvertApiExceptions<AuthorDetailsDto>(apiException);
    }

    return response;
  }

  public async Task<Response<List<AuthorReadOnlyDto>>> GetAsync()
  {
    Response<List<AuthorReadOnlyDto>> response;
    try
    {
      await GetBearerTokenAsync();
      var data = await _client.GetAllAsync();
      response = new Response<List<AuthorReadOnlyDto>>
      {
        Data = data.ToList(),
        Success = true
      };
    }
    catch (ApiException apiException)
    {
      response = await ConvertApiExceptions<List<AuthorReadOnlyDto>>(apiException);
    }

    return response;
  }

  //public async Task<Response<List<AuthorReadOnlyDto>>> GetAsync()
  public async Task<Response<AuthorReadOnlyDtoPagedResult>> GetAsync(QueryParameters queryParams) //cip...66
  {
    //Response<List<AuthorReadOnlyDto>> response;
    Response<AuthorReadOnlyDtoPagedResult> response; //cip...66
    try
    {
      await GetBearerTokenAsync();
      //var data = await _client.AuthorsAllAsync();
      var data = await _client.AuthorsGETAsync(queryParams.StartIndex, queryParams.PageSize, queryParams.SortBy, queryParams.SortDesc, queryParams.Search); //cip...66
      //response = new Response<List<AuthorReadOnlyDto>>
      response = new Response<AuthorReadOnlyDtoPagedResult> //cip...66
      {
        //Data = data.ToList(),
        Data = data, //cip...66
        Success = true
      };
    }
    catch (ApiException apiException)
    {
      //response = ConvertApiExceptions<List<AuthorReadOnlyDto>>(apiException);
      response = await ConvertApiExceptions<AuthorReadOnlyDtoPagedResult>(apiException); //cip...66
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
      response = await ConvertApiExceptions<int>(apiException);
    }

    return response;
  }
}
