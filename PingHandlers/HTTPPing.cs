using Pinger.Interfaces;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
    public class HTTPPing : IPinger
    {
        private readonly string _host;
        private readonly int _statusCode;
        public HTTPPing(string host, int statusCode)
        {
            _host = host;
            _statusCode = statusCode;
        }

        public async Task<bool> Ping()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"http://{_host}");
                return response.StatusCode == (HttpStatusCode)_statusCode;
            }
        }
    }
}