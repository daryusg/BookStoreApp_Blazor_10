namespace BookStoreApp.API.Models.Author;

public class AuthorReadOnlyDto : BaseDto //cip...20
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Bio { get; set; }
}
