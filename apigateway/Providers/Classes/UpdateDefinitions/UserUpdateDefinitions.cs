using MongoDB.Driver;

namespace apigateway
{
    public static class UserUpdateDefinitions
    {
        public static UpdateDefinition<User> ProfilePictureFileName(string fileName) =>
            Builders<User>.Update.Set(e => e.ProfilePictureFileName, fileName);
    }
}