using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using NotNullAttribute = JetBrains.Annotations.NotNullAttribute;

namespace Init.Api.Scripts
{
  public class Script0001 : IScript
  {
    private readonly IMongoCollection<DbVersion> versions;
    private readonly IMongoDatabase database;

    public Script0001(
      [NotNull] IMongoCollection<DbVersion> versions,
      [NotNull] IMongoDatabase database
      )
    {
      this.versions = versions ?? throw new ArgumentNullException(nameof(versions));
      this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task Apply(CancellationToken cancellationToken)
    {
      await CreateVersionCollection(cancellationToken).ConfigureAwait(false);

      await CreateApplicationCollection(cancellationToken).ConfigureAwait(false);
    }

    private Task CreateVersionCollection(CancellationToken cancellationToken)
    {
      var indexBuilder = Builders<DbVersion>.IndexKeys;

      var indexModel = new CreateIndexModel<DbVersion>(
        indexBuilder.Ascending(x => x.Version),
        new CreateIndexOptions { Unique = true }
      );

      return versions.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
    }

    private Task CreateApplicationCollection(CancellationToken cancellationToken)
    {
      var indexBuilder = Builders<Application>.IndexKeys;

      var indexModel = new CreateIndexModel<Application>(
        indexBuilder.Ascending(x => x.Name),
        new CreateIndexOptions { Unique = true }
      );

      var collection = this.database.GetCollection<Application>("applications");

      return collection.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
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
