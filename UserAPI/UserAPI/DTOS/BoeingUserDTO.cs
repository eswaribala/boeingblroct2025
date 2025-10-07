using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserAPI.Models;

namespace UserAPI.DTOS
{
    public class BoeingUserDTO
    {
       
        public long UserId { get; set; }
        public FullNameDTO Name { get; set; }        
        public string Email { get; set; }

    }
}
