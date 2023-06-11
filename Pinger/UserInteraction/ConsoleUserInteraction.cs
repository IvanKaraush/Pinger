using Pinger.Interfaces;
using System;

namespace Pinger.UserInteraction
{
	class ConsoleUserInteraction : IUserInteraction
	{
		public void WaitForAnyKey()
		{
			Console.ReadKey();
		}
	}
}
