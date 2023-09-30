using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Rooms")]
    public class Room
    {
        public Room()
        {
            UsersOfRoom = new HashSet<UserOfRoom>();
            UploadedFiles = new HashSet<UploadedFile>();
            RoomInvitations = new HashSet<RoomInvitation>();
        }

        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string RoomType { get; set; }


        [Required]
        public long LastMessageId { get; set; }
        public virtual Message LastMessage { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public long SchoolYearId { get; set; }
        public virtual SchoolYear SchoolYear { get; set; }

        public virtual ICollection<UserOfRoom> UsersOfRoom { set; get; }
        public virtual ICollection<UploadedFile> UploadedFiles { set; get; }
        public virtual ICollection<RoomInvitation> RoomInvitations { set; get; }
        public virtual ICollection<Message> Messages { set; get; }
    }
}