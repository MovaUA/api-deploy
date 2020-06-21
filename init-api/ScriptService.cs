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

    public ScriptService(
      IMongoCollection<DbVersion> versions,
      IReadOnlyDictionary<int, IScript> scripts
    )
    {
      this.versions = versions;
      this.scripts = scripts;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      return Task.CompletedTask;
    }
  }
}