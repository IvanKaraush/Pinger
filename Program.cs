using Ninject;
using Pinger.Interfaces;
using Pinger.Logger;
using Pinger.PingHandlers;
using System;

namespace Pinger
{
	class Program
	{
		private static IKernel kernel;
		static void Main(string[] args)
		{
			ConfigureService();

			var pinger = kernel.Get<IPingManager>();
			pinger.Run(kernel);

		}

		private static void ConfigureService()
		{
			kernel = new StandardKernel();

			kernel.Bind<ILogger>().To<LogToFile>();
			kernel.Bind<IPingManager>().To<PingManager>();
			kernel.Bind<ICMPPing>().ToSelf();

		}
	}
}
