using Init.Api.Scripts;
using Microsoft.Extensions.Configuration;
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

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureLogging(
          logging =>
          {
            logging.ClearProviders();
            logging.AddConsole(console => { console.TimestampFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ffffff' 'zzz' '"; });
          }
        )
        .ConfigureAppConfiguration(config => { config.AddEnvironmentVariables(prefix: "INITAPI_"); })
        .ConfigureServices(ConfigureServices)
        .UseConsoleLifetime();

    public static void ConfigureServices(
      HostBuilderContext context,
      IServiceCollection services
    )
    {
      services.AddMongoDb(context.Configuration, "mongo");
      services.AddCollection<DbVersion>("versions");
      services.AddSingleton<IVersionRepository, VersionRepository>();
      services.AddSingleton<IScriptRegistry, ScriptRegistry>();
      services.AddHostedService<ScriptService>();

      services.AddScripts(
        (version: 0_0001, script: typeof(Script0001))
      );
    }
  }
}
