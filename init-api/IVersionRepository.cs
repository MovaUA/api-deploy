using System.Threading;
using System.Threading.Tasks;

namespace Init.Api
{
	public interface IVersionRepository
	{
		Task<DbVersion> FindLatest(CancellationToken cancellationToken = default);
	}
}
