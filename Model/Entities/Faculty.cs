using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("Faculties")]
    public class Faculty
    {
        public Faculty()
        {
            Classes = new HashSet<Class>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Class> Classes { set; get; }
    }
}