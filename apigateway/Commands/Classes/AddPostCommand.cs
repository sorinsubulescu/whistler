using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class AddPostCommand : IAddPostCommand
    {
        private readonly AddPostParameters _addPostParameters;
        private readonly string _userId;
        private readonly IPostProvider _postProvider;

        public AddPostCommand(AddPostParameters addPostParameters, string userId, IPostProvider postProvider)
        {
            _addPostParameters = addPostParameters ?? throw new ArgumentNullException(nameof(addPostParameters));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var post = new Post
            {
                Message = _addPostParameters.Message,
                OwnerId = _userId,
                DateCreated = DateTime.UtcNow,
                LikedByUserIds = new List<string>(),
                Comments = new List<Comment>()
            };

            await _postProvider.AddPost(post, cancellationToken).ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}