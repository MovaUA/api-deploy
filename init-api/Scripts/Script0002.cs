using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Init.Api.Business;
using JetBrains.Annotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Init.Api.Scripts
{
  public class Script0002 : IScript
  {
    private readonly IMongoDatabase database;

    public Script0002(
      [JetBrains.Annotations.NotNull] IMongoDatabase database
    )
    {
      this.database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public Task Apply(CancellationToken cancellationToken)
    {
      return CreateUserCollection(cancellationToken);
    }

    private Task CreateUserCollection(CancellationToken cancellationToken)
    {
      var indexBuilder = Builders<User>.IndexKeys;

      var indexModel = new CreateIndexModel<User>(
        indexBuilder.Ascending(x => x.Name),
        new CreateIndexOptions { Unique = true }
      );

      var collection = this.database.GetCollection<User>("users");

      return collection.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken);
    }

    [UsedImplicitly]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class User
    {
      [BsonId]
      public string Id { get; set; }

      [BsonElement("name")]
      public string Name { get; set; }
    }
  }
}
