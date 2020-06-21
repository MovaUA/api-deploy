using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

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
			services.AddSingleton(sp => configuration.GetSection(section).Get<MongoDbSettings>());

			return services.AddSingleton(
				sp =>
				{
					var mongo = sp.GetRequiredService<MongoDbSettings>();

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
								)
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
