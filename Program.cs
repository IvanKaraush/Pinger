using Ninject;
using Pinger.Interfaces;
using Pinger.Logger;
using System;

namespace Pinger
{
	class Program
	{
		private static IKernel kernel;
		static void Main(string[] args)
		{
			ConfigureService();

			var logger = kernel.Get<ILogger>();

			logger.Log("Test");
		}

		private static void ConfigureService()
		{
			kernel = new StandardKernel();

			kernel.Bind<ILogger>().To<LogToFile>();

		}
	}
}
