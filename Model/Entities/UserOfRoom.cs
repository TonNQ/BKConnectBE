using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("UsersOfRoom")]
    public class UserOfRoom
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public long ReadMessageId { get; set; }
        public virtual Message ReadMessage { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public long RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}