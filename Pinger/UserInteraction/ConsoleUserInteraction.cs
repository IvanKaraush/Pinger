using Pinger.Interfaces;
using System;

namespace Pinger.UserInteraction
{
	class ConsoleUserInteraction : IUserInteraction
	{
		public void DisplayMessage(string message)
		{
			Console.WriteLine(message);
		}

		public void WaitForAnyKey()
		{
			Console.ReadKey();
		}
	}
}
