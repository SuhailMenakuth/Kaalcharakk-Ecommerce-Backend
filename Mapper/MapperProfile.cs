using AutoMapper;
using Kaalcharakk.Dtos.AuthenticationDtos;
using Kaalcharakk.Models;

namespace Kaalcharakk.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, RegisterDto>().ReverseMap(); ;
        }
    }
}
