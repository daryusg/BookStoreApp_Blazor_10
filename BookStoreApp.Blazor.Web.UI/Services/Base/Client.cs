namespace BookStoreApp.Blazor.Web.UI.Services.Base;

public partial class Client : IClient
{
  public HttpClient HttpClient
  {
    get => _httpClient; //returned from nswags generated code
  }
}
