using Blazored.LocalStorage;
using BookStoreApp.Blazor.Web.UI.Providers;
using BookStoreApp.Blazor.Web.UI.Services.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Web.UI.Services.Authentication;

//public class AuthenticationService : IAuthenticationService //cip...40
public class AuthenticationService : BaseHttpService, IAuthenticationService //cip...40,67
{
  private readonly IClient _httpClient;
  private readonly ILocalStorageService _localStorage;
  private readonly AuthenticationStateProvider _authenticationStateProvider;

  //public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
  public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
    : base(httpClient, localStorage, authenticationStateProvider, navigationManager) //cip...50 chatgpt fix: pass authentication state provider to base service to log the user out if the token's expired.
  {
    this._httpClient = httpClient;
    this._localStorage = localStorage;
    this._authenticationStateProvider = authenticationStateProvider;
  }

  //public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
  //{
  //  var response = await _httpClient.LoginAsync(loginModel);
  //  //store token
  //  await _localStorage.SetItemAsync("authToken", response.Token);
  //  //change auth state of the app
  //  await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedInAsync();

  //  return true;
  //}
  public async Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel) //cip...67
  {
    Response<AuthResponse> response;
    try
    {
      var authResponse = await _httpClient.LoginAsync(loginModel);
      //store token
      await _localStorage.SetItemAsync("authToken", authResponse.Token);
      //change auth state of the app
      await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedInAsync();
      response = new Response<AuthResponse>
      {
        Data = authResponse,
        Success = true
      };

    }
    catch (ApiException ex)
    {
      //response = new Response<AuthResponse>
      //{
      //  Message = ex.Message,
      //  ValidationErrors = ex.Response,
      //  Success = false
      //};
      response = await ConvertApiExceptions<AuthResponse>(ex);
    }

    return response;
  }


  public async Task LogoutAsync() //cip...41
  {
    //change auth state of the app
    await((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOutAsync();
  }
}
