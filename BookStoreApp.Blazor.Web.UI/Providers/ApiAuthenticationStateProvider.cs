using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStoreApp.Blazor.Web.UI.Providers;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider //cip...40
{
  private readonly ILocalStorageService _localStorage;

  public ApiAuthenticationStateProvider(ILocalStorageService localStorageService)
  {
    this._localStorage = localStorageService;
  }
  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    //user starts as an empty claims principal
    var user = new ClaimsPrincipal(new ClaimsIdentity());
    //retrieve token
    var savedToken = await _localStorage.GetItemAsync<string>("authToken");

    if (string.IsNullOrEmpty(savedToken))
    {
      return new AuthenticationState(user); //return empty claims principal
    }

    var handler = new JwtSecurityTokenHandler();
    var token = handler.ReadJwtToken(savedToken);

    // Check expiry
    if (token.ValidTo < DateTime.UtcNow)
    {
      return new AuthenticationState(user); //return empty claims principal
    }

    // Extract claims
    var identity = new ClaimsIdentity(token.Claims, "jwt");
    user = new ClaimsPrincipal(identity);

    return new AuthenticationState(user); //return claims principal
  }

  public async Task LoggedInAsync()
  {
    var authState = await GetAuthenticationStateAsync();
    NotifyAuthenticationStateChanged(Task.FromResult(authState));
  }

  public async Task LoggedOutAsync()
  {
    await _localStorage.RemoveItemAsync("authToken");
    var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
    var authState = Task.FromResult(new AuthenticationState(anonymousUser));
    NotifyAuthenticationStateChanged(authState);
  }
}
