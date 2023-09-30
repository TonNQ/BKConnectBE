using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Files")]
    public class UploadedFile
    {
        [Key]
        public long Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public DateTime UploadTime { get; set; }

        public long RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}