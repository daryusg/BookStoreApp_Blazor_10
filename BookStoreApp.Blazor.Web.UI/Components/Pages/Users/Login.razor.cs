using BookStoreApp.Blazor.Web.UI.Services.Authentication;
using BookStoreApp.Blazor.Web.UI.Services.Base;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.Web.UI.Components.Pages.Users
{
  public partial class Login //cip...67
  {
    [Inject] IAuthenticationService authService { get; set; }
    [Inject] NavigationManager navManager { get; set; }

    LoginUserDto LoginModel { get; set; } = new();
    string message = string.Empty;

    public async Task HandleLogin()
    {
      //cip...67 removed the try catch as that's handled in the service and i'm returning a response object with the success status. a failed login (401) is handled by BaseHttpService.ConvertApiExceptions and redirects to the login page.
      var response = await authService.AuthenticateAsync(LoginModel);
      if (response.Success) //cip...67
        navManager.NavigateTo("/");
      message = response.Message;
    }

  }
}
