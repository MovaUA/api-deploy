using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Init.Api
{
	public class VersionScript : IVersionScript
	{
		private readonly IScript script;

		public int Version { get; }

		public VersionScript(int version, [NotNull] IScript script)
		{
			Version = version;
			this.script = script ?? throw new ArgumentNullException(nameof(script));
		}

		public Task Apply(CancellationToken cancellationToken = default)
		{
			return this.script.Apply(cancellationToken);
		}
	}
}
