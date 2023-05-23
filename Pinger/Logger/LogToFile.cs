using Pinger.Interfaces;
using System;
using System.IO;

namespace Pinger.Logger
{
	public class LogToFile : ILogger
	{
		public void Log(string message)
		{
			using (var fileStream = new FileStream("log.txt", FileMode.Append))
			{
				using (var streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} {message}");
				}
			}
		}
	}
}
