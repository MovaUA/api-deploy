using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MongoDB.Driver;

namespace Init.Api
{
	public interface IScript
	{
		Task Apply(
			[NotNull] IMongoDatabase database,
			CancellationToken cancellationToken = default
		);
	}
}
