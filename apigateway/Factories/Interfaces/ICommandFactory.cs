using Microsoft.AspNetCore.Http;

namespace apigateway
{
    public interface ICommandFactory
    {
        IAddPostCommand AddPostCommand(AddPostParameters addPostParameters, string userId);

        ILikePostCommand LikePostCommand(string postId, string userId);

        IDislikePostCommand DislikePostCommand(string postId, string userId);

        IDeletePostCommand DeletePostCommand(string postId, string userId);

        IRegisterUserCommand RegisterUserCommand(RegisterUserParameters registerUserParameters);

        IAuthenticateUserCommand AuthenticateUserCommand(AuthenticateUserParameters authenticateUserParameters);

        IRefreshUserTokenCommand RefreshUserTokenCommand(RefreshUserTokenParameters refreshUserTokenParameters);

        ILogoutUserCommand LogoutUserCommand(LogoutUserParameters logoutUserParameters);

        IEditProfilePictureCommand EditProfilePictureCommand(IFormFile file, string userId);

        IEditUserCommand EditUserCommand(EditUserParameters editUserParameters, string userId);

        IFollowUserCommand FollowUserCommand(FollowUserParameters followUserParameters, string userId);

        IUnfollowUserCommand UnfollowUserCommand(UnfollowUserParameters unfollowUserParameters, string userId);

        IAddCommentCommand AddCommentCommand(AddCommentParameters addCommentParameters, string postId, string userId);

        IDeleteCommentCommand DeleteCommentCommand(DeleteCommentParameters deleteCommentParameters, string postId,
            string userId);
    }
}