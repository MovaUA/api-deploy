namespace Init.Api
{
	public interface IScriptRegistry
	{
		bool TryGetNext(int? version, out IScript script);
	}
}
