using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Init.Api.Scripts;

namespace Init.Api
{
	public class ScriptRegistry : IScriptRegistry
	{
		private readonly IReadOnlyDictionary<int, IVersionScript> scripts;

		public ScriptRegistry()
		{
			this.scripts = Create();
		}

		public IEnumerable<IVersionScript> GetNext(int? version)
		{
			return
				(
					from script in this.scripts.Values
					where !version.HasValue || script.Version > version.Value
					select script
				)
				.OrderBy(s => s.Version);
		}

		private static IReadOnlyDictionary<int, IVersionScript> Create()
		{
			return
				new ReadOnlyDictionary<int, IVersionScript>(
					new IVersionScript[]
						{
							new VersionScript<Script0001>(1),
						}
						.ToDictionary(
							s => s.Version
						)
				);
		}
	}
}
