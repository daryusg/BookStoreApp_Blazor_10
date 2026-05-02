using Blazored.LocalStorage;
using BookStoreApp.Blazor.Web.UI.Providers;
using BookStoreApp.Blazor.Web.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Web.UI.Services.Authentication;

public class AuthenticationService : IAuthenticationService //cip...40
{
  private readonly IClient _httpClient;
  private readonly ILocalStorageService _localStorage;
  private readonly AuthenticationStateProvider _authenticationStateProvider;

  public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
  {
    this._httpClient = httpClient;
    this._localStorage = localStorage;
    this._authenticationStateProvider = authenticationStateProvider;
  }

  public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
  {
    var response = await _httpClient.LoginAsync(loginModel);
    //store token
    await _localStorage.SetItemAsync("authToken", response.Token);
    //change auth state of the app
    await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedInAsync();

    return true;
  }
}
