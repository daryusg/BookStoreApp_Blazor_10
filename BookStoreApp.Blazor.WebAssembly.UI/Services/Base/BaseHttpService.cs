using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

public class BaseHttpService //cip...45
{
  private readonly IClient _client;
  private readonly ILocalStorageService _localStorageService;
  private readonly AuthenticationStateProvider _authenticationStateProvider; //cip...50 chatgpt fix: add authentication state provider to base service to log the user out if the token's expired.
  private readonly NavigationManager _navigationManager; //cip...50 chatgpt fix: added to log the user out if the token's expired.

  //public BaseHttpService(IClient client, ILocalStorageService localStorageService)
  public BaseHttpService(IClient client, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
  {
    _client = client;
    _localStorageService = localStorageService;
    _authenticationStateProvider = authenticationStateProvider;
    _navigationManager = navigationManager;
  } //cip...50 chatgpt fix: add authentication state provider and navigation manager to constructor to log the user out if the token's expired.

  //protected Response<T> ConvertApiExceptions<T>(ApiException apiException) //tw uses <Guid> but <T> is less confusing
  protected async Task<Response<T>> ConvertApiExceptions<T>(ApiException apiException) //cip...67 advised by chatgpt to make async to prevent navigation during the logout process.
  {
    switch (apiException.StatusCode)
    {
      case 400: //bad request
        {
          return new Response<T>
          {
            Message = "Validation errors have occurred.",
            ValidationErrors = apiException.Response,
            Success = false
          };
        }
      case 404:
        {
          return new Response<T>
          {
            Message = "The requested resource could not be found.",
            Success = false
          };
        }
      case 401: //copilot then chatgpt fix: if the user is unauthorized, log them out and redirect to the login page
        {
          ((ApiAuthenticationStateProvider)_authenticationStateProvider)?.LoggedOutAsync();
          _navigationManager.NavigateTo("/users/login");

          return new Response<T>
          {
            Message = "Invalid login details or your session has expired. Please log in again.",
            Success = false
          };
        }
      case 403:
        {
          return new Response<T>
          {
            Message = "You are not authorized to perform this action.",
            Success = false
          };
        }
      default:
        {
          return new Response<T> { Message = "Something went wrong. Please try again later.", Success = false };
        }
    }
  }

  protected async Task GetBearerTokenAsync()
  {
    var token = await _localStorageService.GetItemAsync<string>("authToken");
    if (!string.IsNullOrEmpty(token))
    {
      _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
  }
}
