using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Init.Api.Business
{
	public interface IVersionRepository
	{
		[ItemCanBeNull]
		Task<DbVersion> FindLatest(CancellationToken cancellationToken = default);

		Task Insert([NotNull] DbVersion version, CancellationToken cancellationToken = default);
	}
}
