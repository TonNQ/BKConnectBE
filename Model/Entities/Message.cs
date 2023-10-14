using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BKConnectBE.Common.Enumeration;

namespace BKConnectBE.Model.Entities
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        public long Id { get; set; }

        [StringLength(512)]
        public string Content { get; set; }

        [EnumDataType(typeof(MessageType))]
        public string TypeOfMessag { get; set; }

        public DateTime SendTime { get; set; }

        public string SenderId { get; set; }
        public virtual User Sender { get; set; }

        public long RoomId { get; set; }
        public virtual Room Room { get; set; }

        public long? RootMessageId { get; set; }
        public virtual Message? RootMessage { get; set; }

        public virtual ICollection<Message> ReplyMessage { set; get; }
    }
}