using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("SchoolYears")]
    public class SchoolYear : BaseEntity
    {
        public SchoolYear()
        {
            Rooms = new HashSet<Room>();
        }

        [Required]
        public long YearStart { get; set; }

        [Required]
        public long YearEnd { get; set; }

        public virtual ICollection<Room> Rooms { set; get; }
    }
}