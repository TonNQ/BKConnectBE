using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("UploadedFiles")]
    public class UploadedFile
    {
        [Key]
        public long Id { get; set; }

        public string Path { get; set; }

        public DateTime UploadTime { get; set; }

        public long RoomId { get; set; }
        public virtual Room Room { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}