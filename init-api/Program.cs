using System;
using System.Diagnostics.CodeAnalysis;
using Init.Api.Business;
using Init.Api.Scripts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Init.Api
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args)
        .Build()
        .Run();
    }

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public static IHostBuilder CreateHostBuilder([JetBrains.Annotations.NotNull] string[] args)
    {
      if (args == null) throw new ArgumentNullException(nameof(args));

      return Host.CreateDefaultBuilder(args)
             .ConfigureLogging(
               logging =>
               {
                 logging.ClearProviders();
                 logging.AddConsole(console => { console.TimestampFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff' 'zzz' '"; });
               }
             )
             //  .ConfigureAppConfiguration(config => { config.AddEnvironmentVariables(prefix: "INITAPI_"); })
             .ConfigureServices(ConfigureServices)
             .UseConsoleLifetime();
    }

    public static void ConfigureServices(
      [JetBrains.Annotations.NotNull] HostBuilderContext context,
      [JetBrains.Annotations.NotNull] IServiceCollection services
    )
    {
      if (context == null) throw new ArgumentNullException(nameof(context));
      if (services == null) throw new ArgumentNullException(nameof(services));

      services.AddMongoDb(context.Configuration, "mongo");
      services.AddCollection<DbVersion>("versions");
      services.AddSingleton<IVersionRepository, VersionRepository>();
      services.AddSingleton<IScriptRegistry, ScriptRegistry>();
      services.AddHostedService<ScriptService>();

      services.AddScripts(
        (version: 0_001, script: typeof(Script0001))
      );
    }
  }
}
