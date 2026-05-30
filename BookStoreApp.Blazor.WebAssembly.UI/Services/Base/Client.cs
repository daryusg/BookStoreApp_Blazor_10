namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

public partial class Client : IClient
{
  public HttpClient HttpClient
  {
    get => _httpClient; //returned from nswags generated code
  }
}
