using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class DeletePostCommand : IDeletePostCommand
    {
        private readonly string _postId;
        private readonly IPostProvider _postProvider;

        public DeletePostCommand(string postId, IPostProvider postProvider)
        {
            _postId = postId ?? throw new ArgumentNullException(nameof(postId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            await _postProvider.DeletePost(_postId, cancellationToken).ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}