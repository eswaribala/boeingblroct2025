using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAPI.DTOS
{
    public class FullNameDTO
    {
       
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}
