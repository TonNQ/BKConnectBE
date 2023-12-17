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
            RefreshTokens = new HashSet<RefreshToken>();
            UploadedFiles = new HashSet<UploadedFile>();
        }

        [Key]
        public string Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = "sv";

        public DateTime DateOfBirth { get; set; }

        public bool Gender { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(256)]
        public string Avatar { get; set; }

        public DateTime LastOnline { get; set; }
        
        public DateTime UpdatedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }

        public long? ClassId { get; set; }
        public virtual Class? Class { get; set; }

        public string FacultyId { get; set; }
        public virtual Faculty? Faculty { get; set; }

        public virtual ICollection<UserOfRoom> UsersOfRoom { set; get; }
        public virtual ICollection<Message> SentMessages { set; get; }
        public virtual ICollection<RefreshToken> RefreshTokens { set; get; }
        public virtual ICollection<UploadedFile> UploadedFiles { set; get; }
    }
}