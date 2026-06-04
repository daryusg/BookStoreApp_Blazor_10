using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services;

public class BookService : BaseHttpService, IBookService //cip...53
{
  private readonly IClient _client;

  public BookService(IClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider,
    NavigationManager navigationManager) : base(client, localStorageService, authenticationStateProvider, navigationManager)
  {
    this._client = client;
  }

  public async Task<Response<int>> CreateAsync(BookCreateDto book)
  {
    Response<int> response = new() { Success = true };
    try
    {
      await GetBearerTokenAsync();
      await _client.BooksPOSTAsync(book);
    }
    catch (ApiException apiException)
    {
      response = await ConvertApiExceptions<int>(apiException);
    }

    return response;
  }

  public async Task<Response<int>> DeleteAsync(int id)
  {
    Response<int> response = new() { Success = true };
    try
    {
      await GetBearerTokenAsync();
      await _client.BooksDELETEAsync(id);
    }
    catch (ApiException apiException)
    {
      response = await ConvertApiExceptions<int>(apiException);
    }

    return response;
  }

  public async Task<Response<BookDetailsDto>> GetAsync(int id)
  {
    Response<BookDetailsDto> response;
    try
    {
      await GetBearerTokenAsync();
      var data = await _client.BooksGETAsync(id);
      response = new Response<BookDetailsDto>
      {
        Data = data,
        Success = true
      };
    }
    catch (ApiException apiException)
    {
      response = await ConvertApiExceptions<BookDetailsDto>(apiException);
    }

    return response;
  }

  public async Task<Response<List<BookReadOnlyDto>>> GetAsync()
  {
    Response<List<BookReadOnlyDto>> response;
    try
    {
      await GetBearerTokenAsync();
      var data = await _client.BooksAllAsync();
      response = new Response<List<BookReadOnlyDto>>
      {
        Data = data.ToList(),
        Success = true
      };
    }
    catch (ApiException apiException)
    {
      response = await ConvertApiExceptions<List<BookReadOnlyDto>>(apiException);
    }

    return response;
  }

  public async Task<Response<int>> UpdateAsync(int id, BookUpdateDto book)
  {
    Response<int> response = new() { Success = true };
    try
    {
      await GetBearerTokenAsync();
      await _client.BooksPUTAsync(id, book);
    }
    catch (ApiException apiException)
    {
      response = await ConvertApiExceptions<int>(apiException);
    }

    return response;
  }
}
