using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace apigateway
{
    internal sealed class LogoutUserCommand : ILogoutUserCommand
    {
        private readonly IUserProvider _userProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly LogoutUserParameters _logoutUserParameters;
        private readonly IAuthTokenHelper _authTokenHelper;

        public LogoutUserCommand(IUserProvider userProvider, IRefreshTokenProvider tokenProvider,
            LogoutUserParameters logoutUserParameters, IAuthTokenHelper authTokenHelper)
        {
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            _refreshTokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _logoutUserParameters = logoutUserParameters ?? throw new ArgumentNullException(nameof(logoutUserParameters));
            _authTokenHelper = authTokenHelper ?? throw new ArgumentNullException(nameof(authTokenHelper));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ClaimsPrincipal principal;
            try
            {
                principal = _authTokenHelper.GetPrincipalFromToken(_logoutUserParameters.Token);
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine(
                    $"Unable to logout user. {ex}");
                return RestResponse.Error;
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userProvider.GetById(userId, cancellationToken)
                .ConfigureAwait(false);

            if (user == null)
                return RestResponse.Error;

            await _refreshTokenProvider.DeleteByTokenString(_logoutUserParameters.RefreshToken, cancellationToken)
                .ConfigureAwait(false);

            // TODO: maybe move to a background worker since it will only cleanup if the user logs out
            var refreshTokens = await _refreshTokenProvider.GetAllExpiredByUserId(userId, cancellationToken)
                .ConfigureAwait(false);
            await refreshTokens.WhenAllThrottled(e => _refreshTokenProvider.Delete(e.Id, cancellationToken), cancellationToken)
                .ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}