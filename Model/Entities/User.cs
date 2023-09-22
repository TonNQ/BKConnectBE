namespace ChatFriend.Model.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }

    public DateTime BirthDay { get; set; }

    public Account Account { get; set; }
}
