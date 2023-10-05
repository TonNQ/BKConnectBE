using AutoMapper;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.Authentication;
using BKConnectBE.Model.Dtos.RefreshTokenManagement;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Entities;

namespace BKConnectBE.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountDto, User>()
                .ForMember(d => d.Password, opt => opt.MapFrom(x => Security.CreateMD5(x.Password)))
                .ForMember(d => d.Id, opt => opt.MapFrom(x => x.Email.Remove(x.Email.IndexOf('@'))))
                .ForMember(d => d.Name, opt => opt.MapFrom(x => "Sinh viên"))
                .ForMember(d => d.Role, opt => opt.MapFrom(x => x.Email.Contains("sv") ? Role.Student : Role.Teacher))
                .ForMember(d => d.IsActive, opt => opt.MapFrom(x => false))
                .ForMember(d => d.Gender, opt => opt.MapFrom(x => true))
                .ForMember(d => d.Avatar, opt => opt.MapFrom(x => "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.freepik.com%2Ficon%2Fuser_3177440&psig=AOvVaw3Yat5TYhmovhYFL7Lic4hS&ust=1696128288481000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCLjOn7uo0YEDFQAAAAAdAAAAABAq"))
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.Class,
                    opt => opt.MapFrom(src => src.Class.Name)
                );

            CreateMap<RefreshToken, RefreshTokenDto>();
        }
    }
}