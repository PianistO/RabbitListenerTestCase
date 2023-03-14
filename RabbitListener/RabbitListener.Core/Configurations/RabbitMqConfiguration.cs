using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener.Core.Configurations
{
    public class RabbitMqConfiguration
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UrlQueue { get; set; }
    }
}
