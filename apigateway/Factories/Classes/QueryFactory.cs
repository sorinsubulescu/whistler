using System;

namespace apigateway
{
    public sealed class QueryFactory : IQueryFactory
    {
        private readonly IPostProvider _postProvider;

        public QueryFactory(IPostProvider postProvider)
        {
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
        }

        public IGetPostsQuery GetPostsQuery() => new GetPostsQuery(_postProvider);
    }
}