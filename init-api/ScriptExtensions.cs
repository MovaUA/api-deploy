using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Init.Api
{
  public static class ScriptExtensions
  {
    public static IServiceCollection AddScripts(
      this IServiceCollection services,
      params (int version, Type script)[] scripts
    )
    {
      if (scripts is null) throw new ArgumentNullException(nameof(scripts));

      foreach (var (_, script) in scripts)
        services.AddSingleton(script);

      return services.AddSingleton<IEnumerable<(int version, Type type)>>(scripts);
    }
  }
}
