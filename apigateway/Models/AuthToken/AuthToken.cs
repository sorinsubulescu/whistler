using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace apigateway
{
    public sealed class AuthToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public AuthTokenType TokenType { get; set; }

        public string TokenString { get; set; }

        public DateTime Expires { get; set; }
    }
}