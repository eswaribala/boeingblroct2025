using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PolicyHolderFunction.Models
{
    public class Address
    {
        [JsonPropertyName("doorNo")]
        public string DoorNo { get; set; }
        [JsonPropertyName("street")]
        public string Street { get; set; }
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }
    }
}
