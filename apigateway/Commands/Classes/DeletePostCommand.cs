using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class DeletePostCommand : IDeletePostCommand
    {
        private readonly string _postId;
        private readonly string _userId;
        private readonly IPostProvider _postProvider;

        public DeletePostCommand(string postId, string userId, IPostProvider postProvider)
        {
            _postId = postId ?? throw new ArgumentNullException(nameof(postId));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var post = await _postProvider.GetById(_postId, cancellationToken).ConfigureAwait(false);

            if (post.OwnerId != _userId)
            {
                return RestResponse.Error;
            }

            await _postProvider.DeletePost(_postId, cancellationToken).ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}