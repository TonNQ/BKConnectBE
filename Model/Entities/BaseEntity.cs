using System.ComponentModel.DataAnnotations;

namespace ChatFriend.Model.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime UpdatedDate { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
}
