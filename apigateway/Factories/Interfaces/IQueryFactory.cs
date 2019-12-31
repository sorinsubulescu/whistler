namespace apigateway
{
    public interface IQueryFactory
    {
        IGetPostsQuery GetPostsQuery();

        IGetUserQuery GetUserQuery(string userId);

        IGetPostsByUserIdQuery GetPostsByUserIdQuery(string userId);

        IGetPostByIdQuery GetPostByIdQuery(string postId);

        IGetUserBriefInfoQuery GetUserBriefInfoQuery(string userId, string currentUserId);
    }
}