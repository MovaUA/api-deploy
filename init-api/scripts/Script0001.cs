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

      await database.CreateCollectionAsync("versions").ConfigureAwait(false);
    }
  }
}
