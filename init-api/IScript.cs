using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Init.Api
{
	public interface IScript
	{
		int Version { get; }

		Task Apply(
			IMongoDatabase database,
			CancellationToken cancellationToken
		);
	}
}
