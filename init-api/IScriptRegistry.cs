using System.Collections.Generic;

namespace Init.Api
{
	public interface IScriptRegistry
	{
		IEnumerable<IVersionScript> GetNext(int? version);
	}
}
