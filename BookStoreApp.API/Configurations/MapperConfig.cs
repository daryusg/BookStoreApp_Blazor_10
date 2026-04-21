using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Configurations
{
  public class MapperConfig : Profile //cip...20
  {
    public MapperConfig()
    {
      CreateMap<AuthorCreateDto, Author>().ReverseMap(); // Maps AuthorCreateDto to Author and vice versa //cip...20
      CreateMap<AuthorUpdateDto, Author>().ReverseMap();
      CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();

      CreateMap<Book, BookReadOnlyDto>()
        .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}")) // Maps Author's Full Name to AuthorName
        .ReverseMap(); // Maps Book to BookReadOnlyDto and vice versa //cip...25
      CreateMap<Book, BookDetailsDto>()
        .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"))
        .ReverseMap();
      CreateMap<BookCreateDto, Book>().ReverseMap();
      CreateMap<BookUpdateDto, Book>().ReverseMap();
    }
  }
}
