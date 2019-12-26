using System;

namespace apigateway
{
    public sealed class QueryFactory : IQueryFactory
    {
        private readonly IPostProvider _postProvider;
        private readonly IUserProvider _userProvider;

        public QueryFactory(IPostProvider postProvider, IUserProvider userProvider)
        {
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public IGetPostsQuery GetPostsQuery() => new GetPostsQuery(_postProvider, _userProvider);
    }
}