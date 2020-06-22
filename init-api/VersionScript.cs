using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Init.Api
{
	public class VersionScript<TScript> : IVersionScript
		where TScript : IScript, new()
	{
		private TScript script;

		public int Version { get; }

		public VersionScript(int version)
		{
			Version = version;
			this.script = new TScript();
		}

		public Task Apply(
			IMongoDatabase database,
			CancellationToken cancellationToken = default
		)
		{
			if (database == null) throw new ArgumentNullException(nameof(database));

			return this.script.Apply(database, cancellationToken);
		}
	}
}
