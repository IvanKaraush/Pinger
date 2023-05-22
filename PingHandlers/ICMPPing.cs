using Pinger.Interfaces;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
	public class ICMPPing : IPinger
	{
		private readonly NameValueCollection config;
		private readonly ILogger _logger;

		public ICMPPing(ILogger logger)
		{
			_logger = logger;
			config = ConfigurationManager.AppSettings;
		}
		public async Task Ping()
		{
            try
			{
                Ping ping = new Ping();
                await ping.SendPingAsync(config.Get("host"));
				_logger.Log($"{config.Get("host")} OK");
			}
			catch (Exception)
			{
				_logger.Log($"{config.Get("host")} FAILED");
			}
		}
	}
}
