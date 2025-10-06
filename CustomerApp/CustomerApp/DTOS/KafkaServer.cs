using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApp.DTOS
{
    public class KafkaServer
    {
        public string Host { get; set; }
        public string TopicName { get; set; }
        public string Port { get; set; }
    }
}
