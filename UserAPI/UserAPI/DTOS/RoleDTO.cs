using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserAPI.Models;

namespace UserAPI.DTOS
{
    public class RoleDTO
    {

        public long RoleId { get; set; }
        
        public string RoleName { get; set; }

        public long UserId { get; set; }
       
    }
}
