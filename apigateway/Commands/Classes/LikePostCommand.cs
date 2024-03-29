using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class LikePostCommand : ILikePostCommand
    {
        private readonly string _postId;
        private readonly string _userId;
        private readonly IPostProvider _postProvider;

        public LikePostCommand(string postId, string userId, IPostProvider postProvider)
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

            var isPostAlreadyLikedByUser = post.LikedByUserIds.Any(e => e == _userId);

            if (isPostAlreadyLikedByUser)
            {
                return RestResponse.Error;
            }

            await _postProvider.AddUserToLikedByList(_postId, _userId, cancellationToken).ConfigureAwait(false);

            await _postProvider.LikePost(_postId, cancellationToken).ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}