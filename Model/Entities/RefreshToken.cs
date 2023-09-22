using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKConnectBE.Model.Entities
{
    [Table("RefreshTokens")]
    public class RefreshToken
    {
        [Key]
        [Required]
        public long Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        [StringLength(512)]
        public string Token { get; set; }
    }
}