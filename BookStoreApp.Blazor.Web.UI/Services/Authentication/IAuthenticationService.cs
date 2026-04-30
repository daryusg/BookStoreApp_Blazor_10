using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Services.Authentication
{
  public interface IAuthenticationService
  {
    Task<bool> AuthenticateAsync(LoginUserDto loginModel);
  }
}
