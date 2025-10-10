using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Data
{
    public interface IJwtIssuer
    {
        string Issue(string subject, IEnumerable<string> roles = null, IEnumerable<string> scopes = null, TimeSpan? lifetime = null);
    }
}
