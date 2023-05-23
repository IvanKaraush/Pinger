using Pinger.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger.PingHandlers
{
	public class PingManager : IPingManager
	{
		private readonly string _host;
        private readonly double _period;
        private readonly ILogger _logger;
		private readonly IPinger _pinger;
        public PingManager(ILogger logger, IPinger pinger, string host, double period)
		{
			_pinger = pinger;
			_logger = logger;
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

					if (reply) _logger.Log($"{_host} OK");
					else _logger.Log($"{_host} FAILED");

					await Task.Delay(TimeSpan.FromSeconds(_period));
				}
				
			}, cancellationToken);
			Console.WriteLine("Нажмите любую кнопку для остановки пинга");
			Console.ReadKey();

			cancellationTokenSource.Cancel();

		}
	}
}