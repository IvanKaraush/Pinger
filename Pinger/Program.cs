﻿using Microsoft.Extensions.DependencyInjection;
using Pinger.Configuration;
using Pinger.Interfaces;
using Pinger.Logger;
using System.Configuration;
using Pinger.PingHandlers;

namespace Pinger
{
	class Program
	{
		private static AppSettings _appSettings;

		static void Main(string[] args)
		{
			_appSettings = new AppSettings();
			_appSettings.protocol = ConfigurationManager.AppSettings.Get("protocol");
			_appSettings.host = ConfigurationManager.AppSettings.Get("host");
			_appSettings.port = int.Parse(ConfigurationManager.AppSettings.Get("port"));
			_appSettings.period = double.Parse(ConfigurationManager.AppSettings.Get("period"));
			_appSettings.statusCode = int.Parse(ConfigurationManager.AppSettings.Get("statusCode"));
			
			var serviceProvider = ConfigureServices();
			var pinger = serviceProvider.GetService<IPingManager>();
			pinger.Run();

		}

		private static ServiceProvider ConfigureServices()
		{
			var serviceProvider = new ServiceCollection();

			serviceProvider.AddTransient<ILogger, LogToConsole>();
			serviceProvider.AddTransient<ILogger, LogToFile>();
			serviceProvider.AddSingleton(_appSettings);
			serviceProvider.AddTransient<IPinger>(provider =>
			{
				if (_appSettings.protocol.ToLower() == "icmp")
					return new ICMPPing(_appSettings.host);

				if (_appSettings.protocol.ToLower() == "tcp")
					return new TCPPing(_appSettings.host, _appSettings.port);

				if (_appSettings.protocol.ToLower() == "http")
					return new HTTPPing(_appSettings.host, _appSettings.statusCode);

				return new ICMPPing(_appSettings.host);
			});
			serviceProvider.AddTransient<IPingManager, PingManager>();

			return serviceProvider.BuildServiceProvider();
		}
    }
}