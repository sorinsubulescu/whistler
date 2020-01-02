using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class DeleteCommentCommand : IDeleteCommentCommand
    {
        private readonly DeleteCommentParameters _deleteCommentParameters;
        private readonly string _postId;
        private readonly string _userId;
        private readonly IPostProvider _postProvider;

        public DeleteCommentCommand(DeleteCommentParameters deleteCommentParameters, string postId, string userId, IPostProvider postProvider)
        {
            _deleteCommentParameters =
                deleteCommentParameters ?? throw new ArgumentNullException(nameof(deleteCommentParameters));
            _postId = postId ?? throw new ArgumentNullException(nameof(postId));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var post = await _postProvider.GetById(_postId, cancellationToken).ConfigureAwait(false);

            var comment = post.Comments.Single(e => e.Id == _deleteCommentParameters.CommentId);

            if (comment == null)
            {
                return RestResponse.Error;
            }

            if (comment.OwnerId != _userId)
            {
                return RestResponse.Error;
            }

            var deletedSuccessfully = await _postProvider
                .DeleteComment(_postId, _deleteCommentParameters.CommentId, cancellationToken).ConfigureAwait(false);

            return deletedSuccessfully ? RestResponse.Success : RestResponse.Error;
        }
    }
}