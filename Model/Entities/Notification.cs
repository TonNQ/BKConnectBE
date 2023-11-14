using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public long Id { get; set; }

        public DateTime SendTime { get; set; }

        public string Type { get; set; }

        public string Avatar { get; set; }

        public string Content { get; set; }

        public string ReceiverId { get; set; }

        public bool IsRead { get; set; }
    }
}