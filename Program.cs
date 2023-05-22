using Ninject;
using Pinger.Interfaces;
using Pinger.Logger;
using Pinger.PingHandlers;
using System.Collections.Specialized;
using System.Configuration;

namespace Pinger
{
	class Program
	{
		private static IKernel kernel;
        private static NameValueCollection config;
        static void Main(string[] args)
		{
			config = ConfigurationManager.AppSettings;
			ConfigureService();

			var pinger = kernel.Get<IPingManager>();
			pinger.Run();

		}

		private static void ConfigureService()
		{
			kernel = new StandardKernel();

			kernel.Bind<ILogger>().To<LogToFile>();
			kernel.Bind<IPingManager>().To<PingManager>();

			kernel.Bind<IPinger>().To<ICMPPing>().When(c => config.Get("protocol").ToLower() == "icmp");
            kernel.Bind<IPinger>().To<TCPPing>().When(c => config.Get("protocol").ToLower() == "tcp");

			kernel.Bind<IPinger>().To<ICMPPing>();

        }
    }
}