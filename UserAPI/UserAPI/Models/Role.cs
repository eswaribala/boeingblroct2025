using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAPI.Models
{
    [Table("BoeingRoles")]
    public class Role
    {
        [Key]
        [Column("RoleId")]
        public long RoleId { get; set; }
        [Required]
        [Column("RoleName", TypeName = "varchar(20)")]
        [RegularExpression("^[a-zA-Z]{5,20}$", ErrorMessage = "Role name must contain only alphabetic characters.")]
        public string RoleName { get; set; }

        // Foreign key to BoeingUser
        [ForeignKey("BoeingUser")]
        public long UserId { get; set; }
        public BoeingUser User { get; set; }

    }
}
