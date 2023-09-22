namespace ChatFriend.Model.Entities;

public class Account : BaseEntity
{
    public string Email { get; set; }

    public string Password { get; set; }

    public User User { get; set; }

    public int UserId { get; set; }
}
