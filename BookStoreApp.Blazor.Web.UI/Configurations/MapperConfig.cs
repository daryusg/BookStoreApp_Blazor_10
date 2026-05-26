using AutoMapper;
using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Configurations
{
  public class MapperConfig : Profile
  {
    public MapperConfig()
    {
      CreateMap<AuthorDetailsDto, AuthorUpdateDto>().ReverseMap(); //cip...47,48
      CreateMap<BookDetailsDto, BookUpdateDto>()
        .ForMember(
            dest => dest.OriginalImageName,
            opt => opt.MapFrom(src => src.Image)
        )
        .ReverseMap(); //cip...58
    }
  }
}
