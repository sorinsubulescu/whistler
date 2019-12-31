using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace apigateway
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Email { get; set; }

        public string ProfilePictureFileName { get; set; }

        public IEnumerable<string> FollowerUserIds { get; set; }

        public IEnumerable<string> FollowingUserIds { get; set; }
    }
}