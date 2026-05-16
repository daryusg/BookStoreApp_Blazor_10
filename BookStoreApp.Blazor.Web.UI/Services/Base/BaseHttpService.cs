using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace BookStoreApp.Blazor.Web.UI.Services.Base;

public class BaseHttpService //cip...45
{
  private readonly IClient _client;
  private readonly ILocalStorageService _localStorageService;

  public BaseHttpService(IClient client, ILocalStorageService localStorageService)
  {
    _client = client;
    _localStorageService = localStorageService;
  }

  protected Response<T> ConvertApiExceptions<T>(ApiException apiException) //tw uses <Guid> but <T> is less confusing
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
      case 401: //copilot
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
