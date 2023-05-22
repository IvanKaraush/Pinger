using Pinger.Interfaces;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
    public class HTTPPing : IPinger
    {
        private readonly NameValueCollection config;
        public HTTPPing()
        {
            config = ConfigurationManager.AppSettings;
        }

        public async Task<bool> Ping()
        {
            int statusCode = Convert.ToInt32(config.Get("statusCode"));
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"http://{config.Get("host")}");
                if (response.StatusCode == (HttpStatusCode)statusCode) return true;
                else return false;
            }
        }
    }
}
