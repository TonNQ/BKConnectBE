using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Classes")]
    public class Class : BaseEntity
    {
        public Class()
        {
            Users = new HashSet<User>();
        }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public long FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }

        public virtual ICollection<User> Users { set; get; }
    }
}