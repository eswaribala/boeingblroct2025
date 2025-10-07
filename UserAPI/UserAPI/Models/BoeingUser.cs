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
        [Required]
        [Column("Email", TypeName = "varchar(50)")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        
    }
}
