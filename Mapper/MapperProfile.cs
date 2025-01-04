using AutoMapper;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Models;

namespace Kaalcharakk.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<AddProductDto, Product>().ReverseMap();
            CreateMap<ProductViewDto,Product>().ReverseMap();
          
        }   
    }
}
