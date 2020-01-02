using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace apigateway
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string OwnerId { get; set; }

        public string Message { get; set; }
    }
}