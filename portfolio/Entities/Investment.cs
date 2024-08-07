using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Entities;
public class Investment{

    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? ID { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public double Price { get; set; } = 0.00;

    public DateTime Expiration { get; set; } = DateTime.UnixEpoch;

    public bool IsEmpty(){
        return this.ID == null && this.Name == string.Empty && DateTime.Equals(this.Expiration, DateTime.UnixEpoch);
    }
}