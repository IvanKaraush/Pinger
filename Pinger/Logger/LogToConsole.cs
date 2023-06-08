using Pinger.Interfaces;

namespace Pinger.Logger
{
	class LogToConsole : ILogger
	{
		public void Log(string message)
		{
			System.Console.WriteLine(message);
		}
	}
}
