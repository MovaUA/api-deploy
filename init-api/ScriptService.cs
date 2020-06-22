using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Init.Api
{
  public class ScriptService : BackgroundService
  {
    private readonly IMongoCollection<DbVersion> versions;
    private readonly IReadOnlyDictionary<int, IScript> scripts;
    private readonly IHostApplicationLifetime appLifetime;

    public ScriptService(
      IMongoCollection<DbVersion> versions,
      IReadOnlyDictionary<int, IScript> scripts,
      IHostApplicationLifetime appLifetime
    )
    {
      this.versions = versions;
      this.scripts = scripts;
      this.appLifetime = appLifetime;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      try
      {
        return ApplyScripts(stoppingToken);
      }
      finally
      {
        this.appLifetime.StopApplication();
      }
    }

    private Task ApplyScripts(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }
  }
}