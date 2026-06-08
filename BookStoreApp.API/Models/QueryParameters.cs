namespace BookStoreApp.API.Models;

public class QueryParameters
{
    public int StartIndex { get; set; }
    public int PageSize { get; set; } = 20;

    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }

    public string? Search { get; set; }
}