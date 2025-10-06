using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Models
{
    public enum Gender { Male,Female,Other }


    public class Individual:Customer
    {
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
