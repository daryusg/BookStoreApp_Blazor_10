using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Authentication;

public interface IAuthenticationService //cip...40
{
  Task<bool> AuthenticateAsync(LoginUserDto loginModel);
  public Task LogoutAsync(); //cip...41
}
