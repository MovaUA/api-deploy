using MongoDB.Bson.Serialization.Attributes;

namespace Api.Endpoints
{
  public class Application
  {
    [BsonId]
    public string Id { get; set; }

    [BsonElement(elementName: "name")]
    public string Name { get; set; }
  }
}