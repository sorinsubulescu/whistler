using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace apigateway
{
    public class PostProvider : IPostProvider
    {
        private readonly IDatabase _database;

        public PostProvider(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public Task<bool> AddPost(Post post, CancellationToken cancellationToken = default) =>
            _database.InsertOne(post, CollectionName.Posts, cancellationToken);

        public Task<IEnumerable<Post>> GetAll(CancellationToken cancellationToken = default) =>
            _database.GetAll<Post>(CollectionName.Posts, cancellationToken);

        public Task<bool> LikePost(string postId, CancellationToken cancellationToken = default) =>
            _database.UpdateOne(postId, Builders<Post>.Update.Inc(e => e.Likes, 1), CollectionName.Posts,
                cancellationToken);

        public Task<bool> DislikePost(string postId, CancellationToken cancellationToken = default) =>
            _database.UpdateOneByFilter(postId, Builders<Post>.Filter.Gt(e => e.Likes, 0), Builders<Post>.Update.Inc(e => e.Likes, -1), CollectionName.Posts,
                cancellationToken);

        public Task<bool> DeletePost(string postId, CancellationToken cancellationToken = default) =>
            _database.DeleteOne<Post>(postId, CollectionName.Posts, cancellationToken);
    }
}