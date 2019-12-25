namespace apigateway
{
    public interface ICommandFactory
    {
        IAddPostCommand AddPostCommand(AddPostParameters addPostParameters);

        ILikePostCommand LikePostCommand(string postId);

        IDislikePostCommand DislikePostCommand(string postId);

        IDeletePostCommand DeletePostCommand(string postId);

        IRegisterUserCommand RegisterUserCommand(RegisterUserParameters registerUserParameters);

        IAuthenticateUserCommand AuthenticateUserCommand(AuthenticateUserParameters authenticateUserParameters);

        IRefreshUserTokenCommand RefreshUserTokenCommand(RefreshUserTokenParameters refreshUserTokenParameters);

        ILogoutUserCommand LogoutUserCommand(LogoutUserParameters logoutUserParameters);
    }
}