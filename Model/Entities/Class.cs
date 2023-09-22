using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Classes")]
    public class Class
    {
        public Class()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public string FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }

        public virtual ICollection<User> Users { set; get; }
    }
}