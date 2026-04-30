using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Web.UI.Providers
{
  public class ApiAuthenticationStateProvider : AuthenticationStateProvider
  {
    private readonly ILocalStorageService localStorageService;

    public ApiAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
      this.localStorageService = localStorageService;
    }
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      throw new NotImplementedException();
    }
  }
}
