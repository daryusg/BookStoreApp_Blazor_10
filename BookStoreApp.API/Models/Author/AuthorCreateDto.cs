using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Author
{
  public class AuthorCreateDto //cip...20
  {
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(50)]
    public string LastName { get; set; }
    [Required]
    [StringLength(250)]
    public string Bio { get; set; }
  }
}
