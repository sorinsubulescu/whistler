using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class AddPostCommand : IAddPostCommand
    {
        private readonly AddPostParameters _addPostParameters;
        private readonly IPostProvider _postProvider;

        public AddPostCommand(AddPostParameters addPostParameters, IPostProvider postProvider)
        {
            _addPostParameters = addPostParameters ?? throw new ArgumentNullException(nameof(addPostParameters));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var post = new Post
            {
                Message = _addPostParameters.Message
            };

            await _postProvider.AddPost(post, cancellationToken).ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}