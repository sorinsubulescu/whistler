using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class LikePostCommand : ILikePostCommand
    {
        private readonly string _postId;
        private readonly IPostProvider _postProvider;

        public LikePostCommand(string postId, IPostProvider postProvider)
        {
            _postId = postId ?? throw new ArgumentNullException(nameof(postId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            await _postProvider.LikePost(_postId, cancellationToken).ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}