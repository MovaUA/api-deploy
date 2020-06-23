using System.Threading;
using System.Threading.Tasks;

namespace Init.Api.Business
{
	public interface IScript
	{
		Task Apply(CancellationToken cancellationToken = default);
	}
}
