using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Init.Api.Scripts
{
  public class Script0001 : IScript
  {
    public async Task Apply(IMongoDatabase database, CancellationToken cancellationToken)
    {
      if (database == null) throw new ArgumentNullException(nameof(database));

      var indexBuilder = Builders<DbVersion>.IndexKeys;
      var indexModel = new CreateIndexModel<DbVersion>(
        indexBuilder.Ascending(v => v.Version),
         new CreateIndexOptions { Unique = true }
      );
      var versions = database.GetCollection<DbVersion>("versions");
      await versions.Indexes.CreateOneAsync(indexModel, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
  }
}
