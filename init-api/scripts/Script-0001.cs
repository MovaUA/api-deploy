using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Init.Api.Scripts
{
	public class Script0001 : IScript
	{
		public async Task Apply(IMongoDatabase database, CancellationToken cancellationToken)
		{
			if (database == null) throw new ArgumentNullException(nameof(database));

			await CreateVersionCollection(database, cancellationToken).ConfigureAwait(false);

			await CreateApplicationCollection(database, cancellationToken).ConfigureAwait(false);
		}

		private static Task CreateVersionCollection([JetBrains.Annotations.NotNull] IMongoDatabase database, CancellationToken cancellationToken)
		{
			var indexBuilder = Builders<DbVersion>.IndexKeys;

			var indexModel = new CreateIndexModel<DbVersion>(
				indexBuilder.Ascending(v => v.Version),
				new CreateIndexOptions { Unique = true }
			);

			var collection = database.GetCollection<DbVersion>("versions");

			return collection.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
		}

		private static Task CreateApplicationCollection([JetBrains.Annotations.NotNull] IMongoDatabase database, CancellationToken cancellationToken)
		{
			var indexBuilder = Builders<Application>.IndexKeys;

			var indexModel = new CreateIndexModel<Application>(
				indexBuilder.Ascending(v => v.Name),
				new CreateIndexOptions { Unique = true }
			);

			var collection = database.GetCollection<Application>("applications");

			return collection.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
		}

		[UsedImplicitly]
		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
		[SuppressMessage("ReSharper", "UnusedMember.Local")]
		private class DbVersion
		{
			[BsonId]
			public string Id { get; set; }

			[BsonElement("version")]
			public int Version { get; set; }
		}

		[UsedImplicitly]
		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
		[SuppressMessage("ReSharper", "UnusedMember.Local")]
		private class Application
		{
			[BsonId]
			public string Id { get; set; }

			[BsonElement("name")]
			public string Name { get; set; }
		}
	}
}
