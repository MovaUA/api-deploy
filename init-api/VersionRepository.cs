using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Init.Api
{
  public class VersionRepository : IVersionRepository
  {
    private readonly IMongoCollection<DbVersion> collection;

    public VersionRepository(IMongoCollection<DbVersion> collection)
    {
      this.collection = collection;
    }

    public Task<DbVersion> FindLatest(CancellationToken cancellationToken = default)
    {
      return
        collection
          .Find(v => true)
          .SortByDescending(v => v.Version)
          .Limit(1)
          .FirstOrDefaultAsync(cancellationToken);
    }
  }
}