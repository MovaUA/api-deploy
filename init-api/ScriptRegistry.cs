using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Init.Api.Scripts;

namespace Init.Api
{
	public static class ScriptRegistry
	{
		public static IReadOnlyDictionary<int, IScript> Create()
		{
			return
				new ReadOnlyDictionary<int, IScript>(
					new (int version, IScript script)[]
						{
							(1, new Script0001()),
						}
						.ToDictionary(
							s => s.version,
							s => s.script
						)
				);
		}
	}
}
