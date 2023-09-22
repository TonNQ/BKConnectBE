using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Roles")]
    public class Role : BaseEntity
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<User> Users { set; get; }
    }
}