using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Init.Api.Scripts;

namespace Init.Api
{
	public class ScriptRegistry : IScriptRegistry
	{
		private readonly IReadOnlyDictionary<int, IScript> scripts;

		public ScriptRegistry()
		{
			this.scripts = Create();
		}

		public bool TryGetNext(int? version, out IScript script)
		{
			if (this.scripts.Count == 0)
			{
				script = null;
				return false;
			}

			if (!version.HasValue)
			{
				script = this.scripts[this.scripts.Keys.Min()];
				return true;
			}

			var nextVersion =
				this.scripts.Keys
					.Where(v => v > version)
					.OrderBy(v => v)
					.Select(v => (int?) v)
					.FirstOrDefault();

			if (!nextVersion.HasValue)
			{
				script = null;
				return false;
			}

			script = this.scripts[nextVersion.Value];
			return true;
		}

		private static IReadOnlyDictionary<int, IScript> Create()
		{
			return
				new ReadOnlyDictionary<int, IScript>(
					new IScript[]
						{
							new Script0001(),
						}
						.ToDictionary(
							s => s.Version
						)
				);
		}
	}
}
