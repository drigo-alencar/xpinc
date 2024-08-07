using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Entities;

public class Investment{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string ID { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public DateTime Expiration { get; set; }
}