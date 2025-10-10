using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Models
{
    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string Secret { get; set; } = default!;
        public string DefaultRoles { get; set; }
        public string DefaultScopes { get; set; }
    }
}
