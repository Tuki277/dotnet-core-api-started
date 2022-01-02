using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace APIStarted.Models
{
    public class Members : Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Created { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Role { get; set; } // 1 User, // 2 Addmin
        public string Department { get; set; }
        public string Position { get; set; }
    }

    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}