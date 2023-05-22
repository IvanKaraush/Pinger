using Ninject;
using System.Threading.Tasks;

namespace Pinger.Interfaces
{
	public interface IPingManager
	{
		void Run(IKernel kernel);
	}
}
