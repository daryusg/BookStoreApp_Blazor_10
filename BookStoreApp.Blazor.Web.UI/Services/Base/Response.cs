namespace BookStoreApp.Blazor.Web.UI.Services.Base;

public class Response<T> //cip...45
{
  public string Message { get; set; } = string.Empty;
  public string ValidationErrors { get; set; } = string.Empty;
  public bool Success => string.IsNullOrEmpty(ValidationErrors); //intellisense
  //public bool Success { get; set; }

  public T Data { get; set; }
}
