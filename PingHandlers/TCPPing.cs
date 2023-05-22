using Pinger.Interfaces;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
    public class TCPPing : IPinger
    {
        private readonly NameValueCollection config;
        public TCPPing()
        {
            config = ConfigurationManager.AppSettings;
        }

        public async Task<bool> Ping()
        {
            string host = config.Get("host");
            int port = Convert.ToInt32(config.Get("port"));
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    await tcpClient.ConnectAsync(host, port);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}