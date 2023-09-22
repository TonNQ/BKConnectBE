using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            UsersOfRoom = new HashSet<UserOfRoom>();
            SentMessages = new HashSet<Message>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(256)]
        public string Avatar { get; set; }

        [Required]
        public DateTime LastOnline { get; set; }

        
        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public long RoleId { get; set; }
        public virtual Role Role { get; set; }

        public long ClassId { get; set; }
        public virtual Class Class { get; set; }

        public virtual ICollection<UserOfRoom> UsersOfRoom { set; get; }
        public virtual ICollection<Message> SentMessages { set; get; }

        // Có cái ni không hèo
        // public virtual ICollection<Relationship> Relationships { set; get; }
        // public virtual ICollection<FriendRequest> FriendRequests { set; get; }
        // public virtual ICollection<RoomInv> RoomInvs { set; get; }
    }
}