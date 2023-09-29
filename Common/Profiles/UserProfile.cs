using AutoMapper;

namespace BKConnect.Common.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // CreateMap<User, UserDto>()
            //     .ForMember(d => d.Age, opt => opt.MapFrom(x => x.Birthday.GetAgeFromBirthday()));
            // CreateMap<UserDto, User>();
        }
    }
}