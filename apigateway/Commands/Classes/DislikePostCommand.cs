using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class DislikePostCommand : IDislikePostCommand
    {
        private readonly string _postId;
        private readonly string _userId;
        private readonly IPostProvider _postProvider;

        public DislikePostCommand(string postId, string userId, IPostProvider postProvider)
        {
            _postId = postId ?? throw new ArgumentNullException(nameof(postId));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var post = await _postProvider.GetById(_postId, cancellationToken).ConfigureAwait(false);

            if (post == null)
            {
                return RestResponse.Error;
            }

            var isPostAlreadyDislikedByUser = post.LikedByUserIds.All(e => e != _userId);

            if (isPostAlreadyDislikedByUser)
            {
                return RestResponse.Error;
            }

            await _postProvider.RemoveUserFromLikedByList(_postId, _userId, cancellationToken).ConfigureAwait(false);

            await _postProvider.DislikePost(_postId, cancellationToken).ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}