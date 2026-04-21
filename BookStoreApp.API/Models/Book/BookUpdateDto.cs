using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Book;

public class BookUpdateDto : BaseDto //cip...25
{
  [Required]
  public int AuthorId { get; set; }

  [Required]
  [StringLength(50)]
  public string Title { get; set; }

  [Range(1000, int.MaxValue)]
  public int Year { get; set; }

  [Required]
  public string Isbn { get; set; } = null!;

  [StringLength(250, MinimumLength = 10)]
  public string Summary { get; set; }

  public string? Image { get; set; }
  //public string? ImageData { get; set; } //cip...57. rename (note:regen nswag)
  //public string? OriginalImageName { get; set; } //cip...57. (note:regen nswag)

  [Required]
  [Range(0, int.MaxValue)]
  public decimal Price { get; set; }
}
