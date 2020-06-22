using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Init.Api
{
	public class ScriptService : BackgroundService
	{
		private readonly IVersionRepository versionRepository;
		private readonly IScriptRegistry scripts;
		private readonly IHostApplicationLifetime appLifetime;
		private readonly ILogger<ScriptService> logger;

		public ScriptService(
			IVersionRepository versionRepository,
			IScriptRegistry scripts,
			IHostApplicationLifetime appLifetime,
			ILogger<ScriptService> logger
		)
		{
			this.versionRepository = versionRepository;
			this.scripts = scripts;
			this.appLifetime = appLifetime;
			this.logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				this.logger.LogInformation("Applying change scripts...");

				await ApplyScripts(stoppingToken);

				this.logger.LogInformation("Change scripts applied");
			}
			catch (Exception exception)
			{
				this.logger.LogError(1002, exception, "An error occurred");
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

			this.logger.LogInformation(1004, "Latest version: {0}", JsonConvert.SerializeObject(latest));

			while (this.scripts.TryGetNext(latest?.Version, out var script))
			{
				this.logger.LogInformation(1000, "Applying script: version: {0} script: {1}", script.Version, script.GetType().FullName);

				await script.Apply(null, cancellationToken).ConfigureAwait(false);

				latest =
					new DbVersion
					{
						Id = Guid.NewGuid().ToString("N"),
						Version = script.Version
					};

				await this.versionRepository.Insert(latest, cancellationToken).ConfigureAwait(false);

				this.logger.LogInformation(1000, "Script applied: version: {0} script: {1}", script.Version, script.GetType().FullName);
			}
		}
	}
}
