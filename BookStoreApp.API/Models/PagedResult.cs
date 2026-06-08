namespace BookStoreApp.API.Models;

public class PagedResult<T> //20260608 chatgpt potential fix for azure only showing 106 items in azure
{
    public int TotalCount { get; set; }
    public List<T> Items { get; set; } = [];
}
