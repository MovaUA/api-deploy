using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Init.Api.Scripts
{
	public class Script0001 : IScript
	{
		public Task Apply(
			IMongoDatabase database,
			CancellationToken cancellationToken
		)
		{
			return Task.CompletedTask;
		}
	}
}
