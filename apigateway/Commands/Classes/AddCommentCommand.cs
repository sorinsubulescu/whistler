using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace apigateway
{
    public class AddCommentCommand : IAddCommentCommand
    {
        private readonly AddCommentParameters _addCommentParameters;
        private readonly string _postId;
        private readonly string _userId;
        private readonly IPostProvider _postProvider;

        public AddCommentCommand(AddCommentParameters addCommentParameters, string postId, string userId, IPostProvider postProvider)
        {
            _addCommentParameters =
                addCommentParameters ?? throw new ArgumentNullException(nameof(addCommentParameters));
            _postId = postId ?? throw new ArgumentNullException(nameof(postId));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var comment = new Comment
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Message = _addCommentParameters.Message,
                OwnerId = _userId
            };

            var isAddedSuccessfully =
                await _postProvider.AddComment(_postId, comment, cancellationToken).ConfigureAwait(false);

            return isAddedSuccessfully ? RestResponse.Success : RestResponse.Error;
        }
    }
}