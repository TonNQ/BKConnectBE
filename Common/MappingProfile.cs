using AutoMapper;
using BKConnectBE.Model.Dtos;
using BKConnectBE.Model.Entities;

namespace BKConnectBE.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.Class,
                    opt => opt.MapFrom(src => src.Class.Name)
                );
        }
    }
}