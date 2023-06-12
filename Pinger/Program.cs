using Microsoft.Extensions.DependencyInjection;
using Pinger.Configuration;
using Pinger.Interfaces;
using Pinger.Logger;
using Pinger.PingHandlers;
using Pinger.UserInteraction;
using Microsoft.Extensions.Configuration;

namespace Pinger
{
	class Program
	{
		private static AppSettings _appSettings;
		static void Main(string[] args)
		{
			_appSettings = new AppSettings();
			var _configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();
			
			_configuration.GetSection("AppSettings").Bind(_appSettings);
			
			var serviceProvider = ConfigureServices();
			var pinger = serviceProvider.GetService<IPingManager>();
			pinger.Run();

		}

		private static ServiceProvider ConfigureServices()
		{
			var serviceProvider = new ServiceCollection();

			serviceProvider.AddTransient<ILogger, LogToConsole>();
			serviceProvider.AddTransient<ILogger, LogToFile>();
			serviceProvider.AddTransient<IUserInteraction, ConsoleUserInteraction>();
			serviceProvider.AddSingleton(_appSettings);
			serviceProvider.AddTransient<IPinger>(provider =>
			{
				return _appSettings.protocol.ToLower() switch
				{
					"tcp" => new TCPPing(_appSettings.host, _appSettings.port),
					"icmp" => new ICMPPing(_appSettings.host),
					"http" => new HTTPPing(_appSettings.host, _appSettings.statusCode),
					_ => new ICMPPing(_appSettings.host)
				};
			});
			serviceProvider.AddTransient<IPingManager, PingManager>();

			return serviceProvider.BuildServiceProvider();
		}
    }
}