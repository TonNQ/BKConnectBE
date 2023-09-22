using System.ComponentModel.DataAnnotations;

namespace BKConnectBE.Model.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
