using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RabbitListener.Common.Helpers.RequestHelper
{
    public static class RequestHelper
    {
        public static int GetStatusCode(string url)
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;

            return (int)response.StatusCode;
        }
    }
}
