﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Init.Api
{
	public class ScriptService : BackgroundService
	{
		private readonly ILogger<ScriptService> logger;
		private readonly IVersionRepository versionRepository;
		private readonly IScriptRegistry scripts;
		private readonly IHostApplicationLifetime appLifetime;
		private readonly IMongoDatabase database;

		public ScriptService(
			[NotNull] ILogger<ScriptService> logger,
			[NotNull] IVersionRepository versionRepository,
			[NotNull] IScriptRegistry scripts,
			[NotNull] IHostApplicationLifetime appLifetime,
			[NotNull] IMongoDatabase database
		)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.versionRepository = versionRepository ?? throw new ArgumentNullException(nameof(versionRepository));
			this.scripts = scripts ?? throw new ArgumentNullException(nameof(scripts));
			this.appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
			this.database = database ?? throw new ArgumentNullException(nameof(database));
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
				this.logger.LogError(1002, exception, "An error occurred while applying change scripts");
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

			foreach (var script in this.scripts.GetNext(latest?.Version))
			{
				this.logger.LogInformation(1000, "Applying script... version: {0}", script.Version);

				await script.Apply(this.database, cancellationToken).ConfigureAwait(false);

				var newVersion =
					new DbVersion
					{
						Id = Guid.NewGuid().ToString("N"),
						Version = script.Version
					};

				await this.versionRepository.Insert(newVersion, cancellationToken).ConfigureAwait(false);

				this.logger.LogInformation(1000, "Script applied: version: {0}", script.Version);
			}
		}
	}
}
