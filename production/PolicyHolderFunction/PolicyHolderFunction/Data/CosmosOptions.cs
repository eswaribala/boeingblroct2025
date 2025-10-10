using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Data
{
    public sealed class CosmosOptions
    {
        public string AccountEndpoint { get; set; } = default!;
        public string Key { get; set; } = default!;
        public string Database { get; set; } = default!;
        public string Container { get; set; } = default!;

    }
}
