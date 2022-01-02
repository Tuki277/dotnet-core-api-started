using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIStarted.Models
{
    public class Positions
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}