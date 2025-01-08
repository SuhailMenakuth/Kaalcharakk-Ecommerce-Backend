using AutoMapper;
using Kaalcharakk.Dtos.AdressDtos;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Dtos.OrderDtos;
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
            CreateMap<OrderAddressDto , ShippingAddress>().ReverseMap();
            CreateMap<ViewAddressDto , ShippingAddress>().ReverseMap();

           
        }   
    }
}
