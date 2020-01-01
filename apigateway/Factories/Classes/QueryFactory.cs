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

        public IGetPostsQuery GetPostsQuery(string userId) => new GetPostsQuery(userId, _postProvider, _userProvider);

        public IGetUserQuery GetUserQuery(string userId) => new GetUserQuery(userId, _userProvider);

        public IGetPostsByUserIdQuery GetPostsByUserIdQuery(string userId) => new GetPostsByUserIdQuery(userId, _postProvider, _userProvider);

        public IGetPostByIdQuery GetPostByIdQuery(string postId) => new GetPostByIdQuery(postId, _postProvider, _userProvider);

        public IGetUserBriefInfoQuery GetUserBriefInfoQuery(string userId, string currentUserId) =>
            new GetUserBriefInfoQuery(userId, currentUserId, _userProvider);

        public ISearchUsersQuery SearchUsersQuery(string searchTerm) => new SearchUsersQuery(searchTerm, _userProvider);
    }
}