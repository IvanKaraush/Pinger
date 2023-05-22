using Ninject;
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
		public PingManager()
		{
			config = ConfigurationManager.AppSettings;
		}
		public void Run(IKernel kernel)
		{
			IPinger pinger = config.Get("protocol").ToLower() switch
			{
				"icmp" => kernel.Get<ICMPPing>()
			};

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            
			Task.Run(async () =>
			{
				while (!cancellationToken.IsCancellationRequested)
				{
					await pinger.Ping();

					double period = Convert.ToDouble(config.Get("period"));
					await Task.Delay(TimeSpan.FromSeconds(period));
				}
				
			}, cancellationToken);
			Console.WriteLine("Нажмите любую кнопку для остановки пинга");
			Console.ReadLine();

			cancellationTokenSource.Cancel();

		}
	}
}