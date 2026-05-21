using AutoMapper;
using BookStoreApp.Blazor.Web.UI.Services.Base;

namespace BookStoreApp.Blazor.Web.UI.Configurations
{
  public class MapperConfig : Profile
  {
    public MapperConfig()
    {
      CreateMap<AuthorReadOnlyDto, AuthorUpdateDto>().ReverseMap(); //cip...47
    }
  }
}
