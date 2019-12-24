using System;

namespace apigateway
{
    internal sealed class CommandFactory : ICommandFactory
    {
        private readonly IPostProvider _postProvider;
        private readonly IUserProvider _userProvider;
        private readonly IRefreshTokenProvider _tokenProvider;
        private readonly IAuthTokenHelper _authTokenHelper;

        public CommandFactory(IPostProvider postProvider, IUserProvider userProvider,
            IRefreshTokenProvider tokenProvider, IAuthTokenHelper authTokenHelper)
        {
            _postProvider = postProvider ?? throw new ArgumentNullException(nameof(postProvider));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _authTokenHelper = authTokenHelper ?? throw new ArgumentNullException(nameof(authTokenHelper));
        }

        public IAddPostCommand AddPostCommand(AddPostParameters addPostParameters) =>
            new AddPostCommand(addPostParameters, _postProvider);

        public ILikePostCommand LikePostCommand(string postId) => new LikePostCommand(postId, _postProvider);

        public IDislikePostCommand DislikePostCommand(string postId) => new DislikePostCommand(postId, _postProvider);

        public IDeletePostCommand DeletePostCommand(string postId) => new DeletePostCommand(postId, _postProvider);

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
    }
}