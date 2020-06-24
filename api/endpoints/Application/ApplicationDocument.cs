using MongoDB.Bson.Serialization.Attributes;

namespace Api.Endpoints
{
	public class ApplicationDocument
	{
		[BsonId]
		[BsonRequired]
		public string Id { get; set; }

		[BsonElement("name")]
		[BsonRequired]
		public string Name { get; set; }
	}
}
