using MongoDB.Bson.Serialization.Attributes;

namespace Api.Endpoints
{
  public class UserDocument
  {
    [BsonId]
    [BsonRequired]
    public string Id { get; set; }

    [BsonElement("name")]
    [BsonRequired]
    public string Name { get; set; }
  }
}
