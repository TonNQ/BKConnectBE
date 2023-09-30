using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("FriendRequests")]
    public class FriendRequest
    {
        [Key]
        public long Id { get; set; }

        public DateTime SendTime { get; set; }

        public string SenderId { get; set; }
        public virtual User Sender { get; set; }

        public string ReceiverId { get; set; }
        public virtual User Receiver { get; set; }
    }
}