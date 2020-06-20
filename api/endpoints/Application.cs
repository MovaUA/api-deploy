using MongoDB.Bson.Serialization.Attributes;

namespace Api.Endpoints
{
  public class Application
  {
    [BsonId]
    [BsonRequired]
    public string Id { get; set; }

    [BsonElement(elementName: "name")]
    [BsonRequired]
    public string Name { get; set; }
  }
}