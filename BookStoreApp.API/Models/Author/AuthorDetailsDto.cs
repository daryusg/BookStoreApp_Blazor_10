using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Models.Author;

public class AuthorDetailsDto : AuthorReadOnlyDto //cip...48
{
  public List<BookReadOnlyDto> Books { get; set; } = [];
}
