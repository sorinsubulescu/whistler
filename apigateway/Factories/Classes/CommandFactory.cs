using System;
using Microsoft.AspNetCore.Http;

namespace apigateway
{
    internal sealed class CommandFactory : ICommandFactory
    {
        private readonly IPostProvider _postProvider;
        private readonly IUserProvider _userProvider;
        private readonly IRefreshTokenProvider _tokenProvider;
        private readonly IAuthTokenHelper _authTokenHelper;
        private readonly IImageWriter _imageWriter;

        public CommandFactory(IPostProvider postProvider, IUserProvider userProvider,
            IRefreshTokenProvider tokenProvider, IAuthTokenHelper authTokenHelper, IImageWriter imageWriter)
        {
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _authTokenHelper = authTokenHelper ?? throw new ArgumentNullException(nameof(authTokenHelper));
            _imageWriter = imageWriter ?? throw new ArgumentNullException(nameof(imageWriter));
        }

        public IAddPostCommand AddPostCommand(AddPostParameters addPostParameters, string userId) =>
            new AddPostCommand(addPostParameters, userId, _postProvider);

        public ILikePostCommand LikePostCommand(string postId, string userId) => new LikePostCommand(postId, userId, _postProvider);

        public IDislikePostCommand DislikePostCommand(string postId, string userId) => new DislikePostCommand(postId, userId, _postProvider);

        public IDeletePostCommand DeletePostCommand(string postId, string userId) => new DeletePostCommand(postId, userId, _postProvider);

        public IRegisterUserCommand RegisterUserCommand(
            RegisterUserParameters registerUserParameters)
            => new RegisterUserCommand(_userProvider, registerUserParameters);

        public IAuthenticateUserCommand AuthenticateUserCommand(
            AuthenticateUserParameters authenticateUserParameters)
            => new AuthenticateUserCommand(_userProvider, _tokenProvider, authenticateUserParameters, _authTokenHelper);

        public IRefreshUserTokenCommand RefreshUserTokenCommand(
            RefreshUserTokenParameters refreshUserParameters)
            => new RefreshUserTokenCommand(_userProvider, _tokenProvider, refreshUserParameters, _authTokenHelper);

        public ILogoutUserCommand LogoutUserCommand(
            LogoutUserParameters logoutUserParameters)
            => new LogoutUserCommand(_userProvider, _tokenProvider, logoutUserParameters, _authTokenHelper);

        public IEditProfilePictureCommand EditProfilePictureCommand(IFormFile file, string userId) =>
            new EditProfilePictureCommand(file, userId, _imageWriter, _userProvider);

        public IEditUserCommand EditUserCommand(EditUserParameters editUserParameters, string userId) =>
            new EditUserCommand(editUserParameters, userId, _userProvider);

        public IFollowUserCommand FollowUserCommand(FollowUserParameters followUserParameters, string userId) =>
            new FollowUserCommand(followUserParameters, userId, _userProvider);

        public IUnfollowUserCommand UnfollowUserCommand(UnfollowUserParameters unfollowUserParameters, string userId) =>
            new UnfollowUserCommand(unfollowUserParameters, userId, _userProvider);

        public IAddCommentCommand AddCommentCommand(AddCommentParameters addCommentParameters, string postId,
            string userId) => new AddCommentCommand(addCommentParameters, postId, userId, _postProvider);

        public IDeleteCommentCommand DeleteCommentCommand(DeleteCommentParameters deleteCommentParameters, string postId,
            string userId) => new DeleteCommentCommand(deleteCommentParameters, postId, userId, _postProvider);
    }
}