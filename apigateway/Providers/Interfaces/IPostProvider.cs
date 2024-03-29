using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public interface IPostProvider
    {
        Task<bool> AddPost(Post post, CancellationToken cancellationToken = default);

        Task<Post> GetById(string postId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Post>> GetAll(CancellationToken cancellationToken = default);

        Task<IEnumerable<Post>> GetAllByUserId(string userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Post>> GetAllByUserIds(IEnumerable<string> userIds,
            CancellationToken cancellationToken = default);

        Task<bool> LikePost(string postId, CancellationToken cancellationToken = default);

        Task<bool> AddUserToLikedByList(string postId, string userId, CancellationToken cancellationToken = default);

        Task<bool> RemoveUserFromLikedByList(string postId, string userId,
            CancellationToken cancellationToken = default);

        Task<bool> DislikePost(string postId, CancellationToken cancellationToken = default);

        Task<bool> DeletePost(string postId, CancellationToken cancellationToken = default);

        Task<bool> AddComment(string postId, Comment comment, CancellationToken cancellationToken = default);

        Task<bool> DeleteComment(string postId, string commentId, CancellationToken cancellationToken = default);
    }
}