using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener.Common.Helpers.LogHelper
{
    public class LogModel
    {
        public string Url { get; set; }
        public string ServiceName { get; set; }
        public int StatusCode { get; set; }
    }
}
