using MongoDB.Bson.Serialization.Attributes;

namespace Init.Api
{
  public class DbVersion
  {
    [BsonId]
    public string Id { get; set; }

    [BsonElement("version")]
    public int Version { get; set; }
  }
}
