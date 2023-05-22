using Pinger.Interfaces;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
	public class PingManager : IPingManager
	{
		private readonly NameValueCollection config;
        private readonly ILogger _logger;
		private readonly IPinger _pinger;
        public PingManager(ILogger logger, IPinger pinger)
		{
			_pinger = pinger;
			_logger = logger;
			config = ConfigurationManager.AppSettings;
		}
		public void Run()
		{
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            
			Task.Run(async () =>
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					var reply = await _pinger.Ping();

					if (reply) _logger.Log($"{config.Get("host")} OK");
					else _logger.Log($"{config.Get("host")} FAILED");

					double period = Convert.ToDouble(config.Get("period"));
					await Task.Delay(TimeSpan.FromSeconds(period));
				}
				
			}, cancellationToken);
			Console.WriteLine("Нажмите любую кнопку для остановки пинга");
			Console.ReadKey();

			cancellationTokenSource.Cancel();

		}
	}
}