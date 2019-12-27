using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class GetPostsQuery : IGetPostsQuery
    {
        private readonly IPostProvider _postProvider;
        private readonly IUserProvider _userProvider;

        public GetPostsQuery(IPostProvider postProvider, IUserProvider userProvider)
        {
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<GetPostsDto> Execute(CancellationToken cancellationToken = default)
        {
            var posts = (await _postProvider.GetAll(cancellationToken).ConfigureAwait(false)).ToList()
                .OrderByDescending(e => e.DateCreated);

            var postsDto = new List<PostDto>();

            foreach (var post in posts)
            {
                var user = await _userProvider.GetById(post.OwnerId, cancellationToken).ConfigureAwait(false);

                postsDto.Add(new PostDto
                {
                    Id = post.Id,
                    Message = post.Message,
                    LikeCount = post.Likes,
                    Owner = new UserDto
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        ProfilePictureFileName = user.ProfilePictureFileName
                    }
                });
            }

            return new GetPostsDto
            {
                Posts = postsDto
            };
        }
    }
}