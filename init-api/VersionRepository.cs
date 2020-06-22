using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MongoDB.Driver;

namespace Init.Api
{
	public class VersionRepository : IVersionRepository
	{
		private readonly IMongoCollection<DbVersion> collection;

		public VersionRepository([NotNull] IMongoCollection<DbVersion> collection)
		{
			this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
		}

		public Task<DbVersion> FindLatest(CancellationToken cancellationToken = default)
		{
			return this.collection
					   .Find(v => true)
					   .SortByDescending(v => v.Version)
					   .Limit(1)
					   .FirstOrDefaultAsync(cancellationToken);
		}

		public Task Insert([NotNull] DbVersion version, CancellationToken cancellationToken = default)
		{
			if (version == null) throw new ArgumentNullException(nameof(version));

			return this.collection.InsertOneAsync(
				version,
				new InsertOneOptions { BypassDocumentValidation = false },
				cancellationToken
			);
		}
	}
}
