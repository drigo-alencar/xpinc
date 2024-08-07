using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Entities;

public class User
{
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public String ID { get; set; } = ObjectId.GenerateNewId().ToString();
    public string Name { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.String)]
    [JsonConverter(typeof(StringEnumConverter))]
    public UserType Type { get; set; } = UserType.None;

    public IDictionary<string, int> Investments { get; set; } = new Dictionary<string, int>();
}