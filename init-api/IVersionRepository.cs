using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Init.Api
{
	public interface IVersionRepository
	{
		Task<DbVersion> FindLatest(CancellationToken cancellationToken = default);

		Task Insert([NotNull] DbVersion version, CancellationToken cancellationToken = default);
	}
}
