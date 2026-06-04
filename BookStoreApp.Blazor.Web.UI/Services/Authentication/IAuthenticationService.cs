using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services.Authentication;

public interface IAuthenticationService //cip...40
{
  //Task<bool> AuthenticateAsync(LoginUserDto loginModel);
  public Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel); //cip...67
  public Task LogoutAsync(); //cip...41
}
