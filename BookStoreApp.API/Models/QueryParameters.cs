namespace BookStoreApp.API.Models;

public class QueryParameters
{
    public int StartIndex { get; set; }
    public int PageSize { get; set; } = 20;

    public string? SortBy { get; set; }
    public bool? SortDesc { get; set; }  //cip...20260609 chatgpt fix: made nullable

    public string? Search { get; set; }
}