namespace BookStoreApp.API.Models;

public class QueryParameters //20260608 chatgpt potential fix for azure only showing 106 items in azure. also, introduced sorting and searching parameters for future use.
{
    public int StartIndex { get; set; }
    public int PageSize { get; set; } = 20;

    public string? SortBy { get; set; }
    public bool? SortDesc { get; set; }  //cip...20260609 chatgpt fix: made nullable

    public string? Search { get; set; }
}