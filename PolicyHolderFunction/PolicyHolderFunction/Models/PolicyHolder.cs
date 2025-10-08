using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Models
{
    public enum Gender { Male,Female,Other}

    public class PolicyHolder
    {
        public string PolicyNo { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Gender Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


    }
}
