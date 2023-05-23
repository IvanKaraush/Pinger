using Pinger.Interfaces;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
	public class ICMPPing : IPinger
	{
		private readonly string _host;
		public ICMPPing(string host)
		{
			_host = host;
		}
		public async Task<bool> Ping()
		{
            try
			{
                Ping ping = new Ping();
                await ping.SendPingAsync(_host);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
