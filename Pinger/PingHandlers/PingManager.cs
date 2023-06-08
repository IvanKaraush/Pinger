using Pinger.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
	public class PingManager : IPingManager
	{
		private readonly string _host;
        private readonly double _period;
		public IEnumerable<ILogger> _loggers { get; set; }
		private readonly IPinger _pinger;
        public PingManager(IEnumerable<ILogger> loggers, IPinger pinger, string host, double period)
		{
			_pinger = pinger;
			_loggers = loggers;
			_host = host;
			_period = period;
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
						if (reply) item.Log($"{_host} OK");
						else item.Log($"{_host} FAILED");
					}

					await Task.Delay(TimeSpan.FromSeconds(_period));
				}
				
			}, cancellationToken);
			Console.WriteLine("Нажмите любую кнопку для остановки пинга");
			Console.ReadKey();

			cancellationTokenSource.Cancel();

		}
	}
}