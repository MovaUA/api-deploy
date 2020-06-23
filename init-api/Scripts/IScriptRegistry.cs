using System.Collections.Generic;
using JetBrains.Annotations;

namespace Init.Api
{
	public interface IScriptRegistry
	{
		[NotNull]
		IEnumerable<IVersionScript> GetNext(int? version);
	}
}
