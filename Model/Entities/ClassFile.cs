using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("ClassFiles")]
    public class ClassFile
    {
        [Key]
        public long Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public long RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}