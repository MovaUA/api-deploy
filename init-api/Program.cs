using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
          .ConfigureServices(ConfigureServices)
          .UseConsoleLifetime();

    public static void ConfigureServices(
      HostBuilderContext context,
      IServiceCollection services
    )
    {
      services.AddMongoDb(context.Configuration, "mongo");
      services.AddCollection<DbVersion>("versions");
      services.AddSingleton(sp => ScriptRegistry.Create());
      services.AddHostedService<ScriptService>();
    }
  }
}
