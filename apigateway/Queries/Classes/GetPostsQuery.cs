using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class GetPostsQuery : IGetPostsQuery
    {
        private readonly IPostProvider _postProvider;

        public GetPostsQuery(IPostProvider postProvider)
        {
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public async Task<GetPostsDto> Execute(CancellationToken cancellationToken = default)
        {
            var posts = await _postProvider.GetAll(cancellationToken).ConfigureAwait(false);

            return new GetPostsDto
            {
                Posts = posts
            };
        }
    }
}