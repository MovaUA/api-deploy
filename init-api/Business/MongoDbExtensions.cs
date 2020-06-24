using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Init.Api.Business
{
	public static class MongoDbExtensions
	{
		public static IServiceCollection AddMongoDb(
			[NotNull] this IServiceCollection services,
			[NotNull] IConfiguration configuration,
			[NotNull] string section
		)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			if (configuration == null) throw new ArgumentNullException(nameof(configuration));
			if (section == null) throw new ArgumentNullException(nameof(section));

			services.AddSingleton<IMongoDbSettings>(sp => configuration.GetSection(section).Get<MongoDbSettings>());

			return services.AddSingleton(
				sp =>
				{
					var mongo = sp.GetRequiredService<IMongoDbSettings>();

					var logger = sp.GetRequiredService<ILogger<IMongoDbSettings>>();
					logger.LogInformation(1000, JsonConvert.SerializeObject(mongo));

					return new MongoClient(
							new MongoClientSettings
							{
								Credential = MongoCredential.CreateCredential(
									mongo.AuthDb,
									mongo.User,
									mongo.Password
								),
								Server = new MongoServerAddress(
									mongo.Host,
									mongo.Port
								),
								ServerSelectionTimeout = TimeSpan.FromMilliseconds(mongo.SelectTimeoutMS)
							}
						)
						.GetDatabase(mongo.Db);
				}
			);
		}

		public static IServiceCollection AddCollection<T>([NotNull] this IServiceCollection services, [NotNull] string name)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));
			if (name == null) throw new ArgumentNullException(nameof(name));

			return services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<T>(name));
		}
	}
}
