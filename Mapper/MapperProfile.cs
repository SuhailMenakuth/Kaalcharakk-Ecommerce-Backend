using AutoMapper;
using Kaalcharakk.Dtos.AdressDtos;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Dtos.OrderDtos;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Dtos.UserDtos;
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
            CreateMap<OrderViewDto , Order>().ReverseMap();
            CreateMap<UserViewDto, User>().ReverseMap();
            CreateMap<User, MyDetailsDto>()
     .ForMember(dest => dest.ViewUserAddress, opt => opt.MapFrom(src => src.ShippingAddresses))
     .ForSourceMember(src => src.PasswordHash, opt => opt.DoNotValidate()) // Exclude PasswordHash
            .ForSourceMember(src => src.RoleId, opt => opt.DoNotValidate());

            CreateMap<ShippingAddress, ViewAddressDto>().ReverseMap();


        }   
    }
}
