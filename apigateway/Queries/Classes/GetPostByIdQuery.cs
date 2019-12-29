using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class GetPostByIdQuery : IGetPostByIdQuery
    {
        private readonly string _postId;
        private readonly IPostProvider _postProvider;
        private readonly IUserProvider _userProvider;

        public GetPostByIdQuery(string postId, IPostProvider postProvider, IUserProvider userProvider)
        {
            _postId = postId ?? throw new ArgumentNullException(nameof(postId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<PostDto> Execute(CancellationToken cancellationToken = default)
        {
            var post = await _postProvider.GetById(_postId, cancellationToken).ConfigureAwait(false);

            var owner = await _userProvider.GetById(post.OwnerId, cancellationToken).ConfigureAwait(false);

            return new PostDto
            {
                Id = post.Id,
                LikeCount = post.Likes,
                Message = post.Message,
                Owner = new UserDto
                {
                    Email = owner.Email,
                    FullName = owner.FullName,
                    Id = owner.Id,
                    ProfilePictureFileName = owner.ProfilePictureFileName
                }
            };
        }
    }
}