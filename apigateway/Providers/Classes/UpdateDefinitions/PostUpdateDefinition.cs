using MongoDB.Driver;

namespace apigateway
{
    public class PostUpdateDefinition
    {
        public static UpdateDefinition<Post> AddLikedByUserId(string userId) =>
            Builders<Post>.Update.Push(e => e.LikedByUserIds, userId);

        public static UpdateDefinition<Post> RemoveLikedByUserId(string userId) =>
            Builders<Post>.Update.Pull(e => e.LikedByUserIds, userId);

        public static UpdateDefinition<Post> AddComment(Comment comment) =>
            Builders<Post>.Update.Push(e => e.Comments, comment);

        public static UpdateDefinition<Post> DeleteComment(string commentId) =>
            Builders<Post>.Update.PullFilter(e => e.Comments, comment => comment.Id == commentId);
    }
}