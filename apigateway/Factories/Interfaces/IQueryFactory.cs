namespace apigateway
{
    public interface IQueryFactory
    {
        IGetPostsQuery GetPostsQuery(string userId);

        IGetUserQuery GetUserQuery(string userId);

        IGetPostsByUserIdQuery GetPostsByUserIdQuery(string userId);

        IGetPostByIdQuery GetPostByIdQuery(string postId);

        IGetUserBriefInfoQuery GetUserBriefInfoQuery(string userId, string currentUserId);

        ISearchUsersQuery SearchUsersQuery(string searchTerm);
    }
}