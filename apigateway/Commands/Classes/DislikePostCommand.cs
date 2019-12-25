using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class DislikePostCommand : IDislikePostCommand
    {
        private readonly string _postId;
        private readonly IPostProvider _postProvider;

        public DislikePostCommand(string postId, IPostProvider postProvider)
        {
            _postId = postId ?? throw new ArgumentNullException(nameof(postId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            await _postProvider.DislikePost(_postId, cancellationToken).ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}