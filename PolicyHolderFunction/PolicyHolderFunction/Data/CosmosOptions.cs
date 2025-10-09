using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Data
{
    public class CosmosOptions
    {
        public string AccountEndpoint { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string Database { get; set; } = null!;
        public string Container { get; set; } = null!;
    }
}
