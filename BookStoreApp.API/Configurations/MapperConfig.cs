using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;

namespace BookStoreApp.API.Configurations
{
  public class MapperConfig : Profile //cip...20
  {
    public MapperConfig()
    {
      CreateMap<AuthorCreateDto, Author>().ReverseMap(); // Maps AuthorCreateDto to Author and vice versa
      CreateMap<AuthorUpdateDto, Author>().ReverseMap();
      CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();
    }
  }
}
