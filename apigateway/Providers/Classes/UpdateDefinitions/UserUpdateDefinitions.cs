using MongoDB.Driver;

namespace apigateway
{
    public static class UserUpdateDefinitions
    {
        public static UpdateDefinition<User> ProfilePictureFileName(string fileName) =>
            Builders<User>.Update.Set(e => e.ProfilePictureFileName, fileName);

        public static UpdateDefinition<User> FullName(string fullName) =>
            Builders<User>.Update.Set(e => e.FullName, fullName);

        public static UpdateDefinition<User> AddFollowingUser(string userToFollowId) =>
            Builders<User>.Update.Push(e => e.FollowingUserIds, userToFollowId);

        public static UpdateDefinition<User> AddFollowerUser(string followerUserId) =>
            Builders<User>.Update.Push(e => e.FollowerUserIds, followerUserId);

        public static UpdateDefinition<User> RemoveFollowingUser(string userToUnfollowId) =>
            Builders<User>.Update.Pull(e => e.FollowingUserIds, userToUnfollowId);

        public static UpdateDefinition<User> RemoveFollowerUser(string unfollowerUserId) =>
            Builders<User>.Update.Pull(e => e.FollowerUserIds, unfollowerUserId);
    }
}