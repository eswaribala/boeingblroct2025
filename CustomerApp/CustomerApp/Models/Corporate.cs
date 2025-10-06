using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.Models
{
    public enum CompanyType { PrivateLimited, PublicLimited, Partnership, SoleProprietorship, NonProfit }
    public class Corporate: Customer
    {
        public CompanyType CompanyType { get; set; }
    }
}
