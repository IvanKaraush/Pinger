using Pinger.Configuration;
using Pinger.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
	public class PingManager : IPingManager
	{
		private readonly AppSettings _appSettings;
		public IEnumerable<ILogger> _loggers { get; set; }
		private readonly IPinger _pinger;
        public PingManager(IEnumerable<ILogger> loggers, IPinger pinger, AppSettings appSettings)
		{
			_pinger = pinger;
			_loggers = loggers;
			_appSettings = appSettings;
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

					foreach (var item in _loggers)
					{
						if (reply) item.Log($"{_appSettings.host} OK");
						else item.Log($"{_appSettings.host} FAILED");
					}

					await Task.Delay(TimeSpan.FromSeconds(_appSettings.period));
				}
				
			}, cancellationToken);
			
			Console.ReadKey();

			cancellationTokenSource.Cancel();

		}
	}
}