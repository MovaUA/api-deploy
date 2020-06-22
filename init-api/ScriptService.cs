using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Init.Api
{
  public class ScriptService : BackgroundService
  {
    private readonly IVersionRepository versionRepository;
    private readonly IReadOnlyDictionary<int, IScript> scripts;
    private readonly IHostApplicationLifetime appLifetime;
    private readonly ILogger<ScriptService> logger;
    private readonly IMongoDbSettings settings;

    public ScriptService(
      IVersionRepository versionRepository,
      IReadOnlyDictionary<int, IScript> scripts,
      IHostApplicationLifetime appLifetime,
      ILogger<ScriptService> logger,
      IMongoDbSettings settings
    )
    {
      this.versionRepository = versionRepository;
      this.scripts = scripts;
      this.appLifetime = appLifetime;
      this.logger = logger;
      this.settings = settings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      try
      {
        this.logger.LogInformation(1000, "Service started");

        this.logger.LogInformation(1001, "Settings:{0}{1}", Environment.NewLine, JsonConvert.SerializeObject(this.settings));

        await ApplyScripts(stoppingToken);
      }
      catch (Exception exception)
      {
        this.logger.LogError(1002, exception, "Service failed");
        throw;
      }
      finally
      {
        this.logger.LogInformation(1003, "Service completed");
        this.appLifetime.StopApplication();
      }
    }

    private async Task ApplyScripts(CancellationToken cancellationToken)
    {
      var latestVersion = await this.versionRepository.FindLatest();

      this.logger.LogInformation(1004, "Latest version: ", JsonConvert.SerializeObject(latestVersion));



    }
  }
}