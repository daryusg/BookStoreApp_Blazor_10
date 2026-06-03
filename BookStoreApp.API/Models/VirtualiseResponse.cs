namespace BookStoreApp.API.Models;

public class VirtualiseResponse<T> //cip...66
{
  public List<T> Items { get; set; }
  public int TotalCount { get; set; }
}
