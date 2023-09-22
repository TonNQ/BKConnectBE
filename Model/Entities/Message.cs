using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Messages")]
    public class Message : BaseEntity
    {
        [Required]
        [StringLength(512)]
        public string Content { get; set; }

        [Required]
        public string TypeOfMessag { get; set; }

        [Required]
        public DateTime SendTime { get; set; }

        public string SenderId { get; set; }
        public virtual User Sender { get; set; }

        public long RoomId { get; set; }
        public virtual Room Room { get; set; }

        public long RootMessageId { get; set; }
        public virtual Message RootMessage { get; set; }

        public virtual ICollection<Message> ReplyMessage { set; get; }
    }
}