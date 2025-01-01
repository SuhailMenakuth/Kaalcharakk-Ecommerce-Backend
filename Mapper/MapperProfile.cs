using AutoMapper;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Dtos.ProductDtos;
using Kaalcharakk.Models;
using Kaalcharakk.Models.Product;

namespace Kaalcharakk.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
                //.ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.productSizes.Select(s => new ProductSize { Size = s.Size, StockQuantity = s.StockQuantity }).ToList()));
               // .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.Images.Select(i => new ProductImage { ImageUrl = i }).ToList())); ;
            
        }
    }
}
