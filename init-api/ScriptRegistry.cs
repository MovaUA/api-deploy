using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Init.Api.Scripts;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Init.Api
{
  public class ScriptRegistry : IScriptRegistry
  {
    private readonly IReadOnlyDictionary<int, IVersionScript> scripts;

    public ScriptRegistry(
      [NotNull] IServiceProvider serviceProvider,
      [NotNull] IEnumerable<(int version, Type type)> scripts
    )
    {
      this.scripts = new ReadOnlyDictionary<int, IVersionScript>(
        scripts.Select(
          s =>
             (IVersionScript)new VersionScript(
                 s.version,
                 (IScript)serviceProvider.GetRequiredService(s.type)
           )
        )
        .ToDictionary(
          s => s.Version
        )
      );
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
  }
}
