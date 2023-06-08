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
			kernel.Bind<ILogger>().To<LogToConsole>();


			kernel.Bind<IPingManager>().To<PingManager>()
				.WithConstructorArgument("host", config.Get("host"))
				.WithConstructorArgument("period", double.Parse(config.Get("period")));

            kernel.Bind<IPinger>().To<ICMPPing>()
				.When(c => config.Get("protocol").ToLower() == "icmp")
				.WithConstructorArgument("host", config.Get("host"));
            
			kernel.Bind<IPinger>().To<TCPPing>()
				.When(c => config.Get("protocol").ToLower() == "tcp")
                .WithConstructorArgument("port", int.Parse(config.Get("port")))
				.WithConstructorArgument("host", config.Get("host"));
            
			kernel.Bind<IPinger>().To<HTTPPing>()
				.When(c => config.Get("protocol").ToLower() == "http")
                .WithConstructorArgument("statusCode", int.Parse(config.Get("statusCode")))
				.WithConstructorArgument("host", config.Get("host"));

            kernel.Bind<IPinger>().To<ICMPPing>()
				.WithConstructorArgument("host", config.Get("host"));
		}
    }
}