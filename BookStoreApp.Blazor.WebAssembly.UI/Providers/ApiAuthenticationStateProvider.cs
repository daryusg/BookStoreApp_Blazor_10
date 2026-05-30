using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStoreApp.Blazor.WebAssembly.UI.Providers;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider //cip...40
{
  private readonly ILocalStorageService _localStorage;

  public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
  {
    this._localStorage = localStorage;
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
      await _localStorage.RemoveItemAsync("authToken"); //cip...50 chatgpt fix: remove expired token from local storage
      return new AuthenticationState(user); //return empty claims principal
    }

    // Extract claims
    var claims = token.Claims.ToList();
    //add the Name claim to the claims collection, so that we can use @context.User.Identity.Name in our components //cip...41
    claims.Add(new Claim(ClaimTypes.Name, token.Subject));

    var identity = new ClaimsIdentity(claims, "jwt");
    user = new ClaimsPrincipal(identity);

    return new AuthenticationState(user); //return claims principal
  }

  public async Task LoggedInAsync()
  {
    var authState = await GetAuthenticationStateAsync();
    NotifyAuthenticationStateChanged(Task.FromResult(authState)); //let the app know that the user is now logged in, so that it can update the UI accordingly
  }

  public async Task LoggedOutAsync()
  {
    await _localStorage.RemoveItemAsync("authToken"); //remove the token from local storage
    var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
    var authState = Task.FromResult(new AuthenticationState(anonymousUser));
    NotifyAuthenticationStateChanged(authState); //let the app know that the user is now logged out, so that it can update the UI accordingly
  }
}
