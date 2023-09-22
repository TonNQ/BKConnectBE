using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Files")]
    public class UploadedFile : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime UploadTime { get; set; }

        public long RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}