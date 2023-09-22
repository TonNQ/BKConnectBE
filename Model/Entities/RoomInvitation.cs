using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("RoomInvitations")]
    public class RoomInvitation : BaseEntity
    {
        [Required]
        public DateTime SendTime { get; set; }

        public long RoomId { get; set; }
        public virtual Room Room { get; set; }

        public string SenderId { get; set; }
        public virtual User Sender { get; set; }

        public string ReceiverId { get; set; }
        public virtual User Receiver { get; set; }
    }
}