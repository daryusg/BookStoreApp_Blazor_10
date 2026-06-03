namespace BookStoreApp.Blazor.Web.UI.Models;

public class QueryParameters //cip...66
{
  private int _pageSize;
  public int StartIndex { get; set; }
  public int PageSize
  {
    get => _pageSize;
    set => _pageSize = value;
  }
}
