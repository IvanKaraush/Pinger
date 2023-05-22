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
        private readonly string _host;
        private readonly int _port;

        public TCPPing(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public async Task<bool> Ping()
        {
            try
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    await tcpClient.ConnectAsync(_host, _port);
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