using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Init.Api
{
  public class ScriptService : BackgroundService
  {
    private readonly ILogger<ScriptService> logger;
    private readonly IVersionRepository versionRepository;
    private readonly IScriptRegistry scripts;
    private readonly IHostApplicationLifetime appLifetime;

    public ScriptService(
      [NotNull] ILogger<ScriptService> logger,
      [NotNull] IVersionRepository versionRepository,
      [NotNull] IScriptRegistry scripts,
      [NotNull] IHostApplicationLifetime appLifetime
    )
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.versionRepository = versionRepository ?? throw new ArgumentNullException(nameof(versionRepository));
      this.scripts = scripts ?? throw new ArgumentNullException(nameof(scripts));
      this.appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      try
      {
        this.logger.LogInformation(1000, "Applying scripts...");

        await ApplyScripts(stoppingToken);

        this.logger.LogInformation(1001, "Scripts applied");
      }
      catch (Exception exception)
      {
        this.logger.LogError(1002, exception, "An error occurred while applying scripts");
        throw;
      }
      finally
      {
        this.appLifetime.StopApplication();
      }
    }

    private async Task ApplyScripts(CancellationToken cancellationToken)
    {
      var latest = await this.versionRepository.FindLatest(cancellationToken).ConfigureAwait(false);

      this.logger.LogInformation(1003, "Latest version: {0}", JsonConvert.SerializeObject(latest));

      foreach (var script in this.scripts.GetNext(latest?.Version))
      {
        this.logger.LogInformation(1004, "Applying script... version: {0}", script.Version);

        await script.Apply(cancellationToken).ConfigureAwait(false);

        var newVersion =
          new DbVersion
          {
            Id = Guid.NewGuid().ToString("N"),
            Version = script.Version
          };

        await this.versionRepository.Insert(newVersion, cancellationToken).ConfigureAwait(false);

        this.logger.LogInformation(1005, "Script applied: version: {0}", script.Version);
      }
    }
  }
}
