using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BKConnectBE.Common.Enumeration;

namespace BKConnectBE.Model.Entities
{
    [Table("Rooms")]
    public class Room
    {
        public Room()
        {
            UsersOfRoom = new HashSet<UserOfRoom>();
            UploadedFiles = new HashSet<UploadedFile>();
            Messages = new HashSet<Message>();
        }

        [Key]
        public long Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [EnumDataType(typeof(RoomType))]
        public string RoomType { get; set; }

        public string Avatar { get; set; }

        public DateTime UpdatedDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<UserOfRoom> UsersOfRoom { set; get; }
        public virtual ICollection<UploadedFile> UploadedFiles { set; get; }
        public virtual ICollection<Message> Messages { set; get; }
    }
}