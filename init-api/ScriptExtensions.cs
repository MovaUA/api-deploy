using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Init.Api
{
	public static class ScriptExtensions
	{
		public static IServiceCollection AddScripts(
			[NotNull] this IServiceCollection services,
			[NotNull] params (int version, Type script)[] scripts
		)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			if (scripts == null) throw new ArgumentNullException(nameof(scripts));

			foreach (var (_, script) in scripts)
				services.AddSingleton(script);

			return services.AddSingleton<IEnumerable<(int version, Type type)>>(scripts);
		}
	}
}
