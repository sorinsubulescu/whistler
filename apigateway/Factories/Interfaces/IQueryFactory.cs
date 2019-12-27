namespace apigateway
{
    public interface IQueryFactory
    {
        IGetPostsQuery GetPostsQuery();

        IGetCurrentUserQuery GetCurrentUserQuery(string userId);

        IGetPostsByUserIdQuery GetPostsByUserIdQuery(string userId);

        IGetPostByIdQuery GetPostByIdQuery(string postId);
    }
}