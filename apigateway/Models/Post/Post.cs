using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace apigateway
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Message { get; set; }

        public int Likes { get; set; } = 0;

        public IEnumerable<string> LikedByUserIds { get; set; }

        public string OwnerId { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}