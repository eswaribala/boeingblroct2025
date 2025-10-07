using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAPI.Models
{
    [Table("BoeingUsers")]
    public class BoeingUser
    {
        [Key]
        [Column("UserId")]
        public long UserId { get; set; }
        public FullName Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
