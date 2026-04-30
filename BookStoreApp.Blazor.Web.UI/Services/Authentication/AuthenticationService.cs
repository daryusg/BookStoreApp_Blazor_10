using Blazored.LocalStorage;
using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services.Authentication
{
  public class AuthenticationService : IAuthenticationService //cip...40
  {
    private readonly IClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(IClient httpClient, ILocalStorageService localStorage)
    {
      this._httpClient = httpClient;
      this._localStorage = localStorage;
    }

    public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
    {
      var response = await _httpClient.LoginAsync(loginModel);
      //store token
      await _localStorage.SetItemAsync("authToken", response.Token);
      //change auth state of the app

      return true;
    }
  }
}
