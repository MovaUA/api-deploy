using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Api
{
	public static class MongoDbExtensions
	{
		public static IServiceCollection AddMongoDb(
			this IServiceCollection services,
			IConfiguration configuration,
			string section
		)
		{
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

		public static IServiceCollection AddCollection<T>(this IServiceCollection services, string name)
		{
			return services.AddSingleton(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<T>(name));
		}
	}
}
