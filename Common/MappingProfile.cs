using AutoMapper;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.Authentication;
using BKConnectBE.Model.Dtos.ClassManagement;
using BKConnectBE.Model.Dtos.FacultyManagement;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Model.Dtos.NotificationManagement;
using BKConnectBE.Model.Dtos.RefreshTokenManagement;
using BKConnectBE.Model.Dtos.RoomManagement;
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
                .ForMember(d => d.DateOfBirth, opt => opt.MapFrom(x => DateTime.UtcNow.AddHours(7)))
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(x => DateTime.UtcNow.AddHours(7)));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Class.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.FacultyId, opt => opt.MapFrom(src => src.Class.FacultyId))
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Class.Faculty.Name));

            CreateMap<UserInputDto, User>()
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.ClassId))
                .ForPath(dest => dest.Class.FacultyId, opt => opt.MapFrom(src => src.FacultyId));

            CreateMap<User, FriendDto>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.Name : null));

            CreateMap<UserOfRoom, MemberOfRoomDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin));

            CreateMap<RefreshToken, RefreshTokenDto>();

            CreateMap<Message, ReceiveMessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.Name))
                .ForMember(dest => dest.RootMessageContent, opt => opt.MapFrom(src => src.RootMessage.Content));

            CreateMap<SendMessageDto, Message>();

            CreateMap<Message, FileDto>();
            
            CreateMap<ClassFile, FileDto>();

            CreateMap<Class, ClassDto>()
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.FacultyId, opt => opt.MapFrom(src => src.FacultyId))
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name));

            CreateMap<Faculty, FacultyDto>()
                .ForMember(dest => dest.FacultyId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Room, RoomDetailDto>()
                .ForMember(dest => dest.TotalMember, opt => opt.MapFrom(x => x.UsersOfRoom.Count))
                .ForMember(dest => dest.IsOnline, opt => opt.MapFrom(x => false))
                .ForMember(dest => dest.FriendId, opt => opt.MapFrom(x => ""));

            CreateMap<Room, GroupRoomDto>()
                .ForMember(dest => dest.TotalMember, opt => opt.MapFrom(x => x.UsersOfRoom.Count));

            CreateMap<AddGroupRoomDto, Room>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7)))
                .ForMember(dest => dest.UsersOfRoom, opt => opt.MapFrom(src => new List<UserOfRoom>()))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new List<Message>()));

            CreateMap<Notification, ReceiveNotificationDto>()
                .ForMember(dest => dest.NotificationType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.Content));
        }
    }
}