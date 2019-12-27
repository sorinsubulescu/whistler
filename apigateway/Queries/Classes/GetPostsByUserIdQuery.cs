using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class GetPostsByUserIdQuery : IGetPostsByUserIdQuery
    {
        private readonly string _userId;
        private readonly IPostProvider _postProvider;
        private readonly IUserProvider _userProvider;

        public GetPostsByUserIdQuery(string userId, IPostProvider postProvider, IUserProvider userProvider)
        {
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }
        public async Task<GetPostsDto> Execute(CancellationToken cancellationToken = default)
        {
            var posts = (await _postProvider.GetAllByUserId(_userId, cancellationToken).ConfigureAwait(false)).ToList()
                .OrderByDescending(e => e.DateCreated);

            var user = await _userProvider.GetById(_userId, cancellationToken).ConfigureAwait(false);

            var postsDto = posts.Select(post => new PostDto
            {
                Id = post.Id, Message = post.Message, LikeCount = post.Likes,
                Owner = new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    ProfilePictureFileName = user.ProfilePictureFileName
                }
            }).ToList();

            return new GetPostsDto
            {
                Posts = postsDto
            };
        }
    }
}