using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Relationships")]
    public class Relationship
    {
        [Key]
        public long Id { get; set; }

        public string BlockBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string User1Id { get; set; }
        public virtual User User1 { get; set; }

        public string User2Id { get; set; }
        public virtual User User2 { get; set; }
    }
}