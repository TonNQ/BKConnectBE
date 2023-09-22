using ChatFriend.Model.Entities;

namespace ChatFriend.Model.Dtos;

public class UserDto
{
    public UserDto() { }

    public UserDto(User user)
    {
        Name= user.Name;
        Id = user.Id;
        BirthDay = user.BirthDay;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime BirthDay { get; set; }
}
