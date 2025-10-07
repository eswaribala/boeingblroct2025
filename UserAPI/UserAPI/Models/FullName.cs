using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAPI.Models
{
    [Owned]
    public class FullName
    {
        [Required]
        [Column("FirstName",TypeName ="varchar(25)")]
        [RegularExpression("^[a-zA-Z]{5,25}$", ErrorMessage = "First name must contain only alphabetic characters.")]
        public string FirstName { get; set; }
        [Required]
        [Column("LastName", TypeName = "varchar(25)")]
        [RegularExpression("^[a-zA-Z]{5,25}$", ErrorMessage = "Last name must contain only alphabetic characters.")]
        public string LastName { get; set; }
    }
}
