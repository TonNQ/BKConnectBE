using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("SchoolYears")]
    public class SchoolYear
    {
        public SchoolYear()
        {
            Rooms = new HashSet<Room>();
        }

        [Key]
        public long Id { get; set; }

        public string Year { get; set; }

        public string Schemes { get; set; }

        public virtual ICollection<Room> Rooms { set; get; }
    }
}