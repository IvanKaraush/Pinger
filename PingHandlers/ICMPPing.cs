using Pinger.Interfaces;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
	public class ICMPPing : IPinger
	{
		private readonly NameValueCollection config;
		public ICMPPing()
		{
			config = ConfigurationManager.AppSettings;
		}
		public async Task<bool> Ping()
		{
            try
			{
                Ping ping = new Ping();
                await ping.SendPingAsync(config.Get("host"));
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
