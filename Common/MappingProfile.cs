using AutoMapper;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.Authentication;
using BKConnectBE.Model.Dtos.ClassManagement;
using BKConnectBE.Model.Dtos.FacultyManagement;
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
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Class.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.FacultyId, opt => opt.MapFrom(src => src.Class.FacultyId))
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Class.Faculty.Name));

            CreateMap<UserInputDto, User>()
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.ClassId))
                .ForPath(dest => dest.Class.FacultyId, opt => opt.MapFrom(src => src.FacultyId));

            CreateMap<RefreshToken, RefreshTokenDto>();

            CreateMap<Class, ClassDto>()
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.FacultyId, opt => opt.MapFrom(src => src.FacultyId))
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name));

            CreateMap<Faculty, FacultyDto>()
                .ForMember(dest => dest.FacultyId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Name));
        }
    }
}