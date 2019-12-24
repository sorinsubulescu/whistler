using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace apigateway
{
    internal sealed class RefreshUserTokenCommand : IRefreshUserTokenCommand
    {
        private readonly IUserProvider _userProvider;
        private readonly IRefreshTokenProvider _tokenProvider;
        private readonly RefreshUserTokenParameters _refreshUserTokenParameters;
        private readonly IAuthTokenHelper _authTokenHelper;

        public RefreshUserTokenCommand(IUserProvider userProvider, IRefreshTokenProvider tokenProvider,
            RefreshUserTokenParameters refreshUserParameters, IAuthTokenHelper authTokenHelper)
        {
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _refreshUserTokenParameters = refreshUserParameters ??
                                          throw new ArgumentNullException(nameof(refreshUserParameters));
            _authTokenHelper = authTokenHelper ?? throw new ArgumentNullException(nameof(authTokenHelper));
        }

        public async Task<RefreshUserTokenDto> Execute(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ClaimsPrincipal principal;

            try
            {
                principal = _authTokenHelper.GetPrincipalFromToken(_refreshUserTokenParameters.Token);
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine(
                    $"Unable to validate JWT token. {ex}");

                return new RefreshUserTokenDto
                {
                    Result = RefreshUserTokenDto.ResultType.Failed,
                    ResponseKey = UiLocalizationKey.RefreshUserTokenFailed,
                    ResponseMessage = UiLocalizationMessage.RefreshUserTokenFailed
                };
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userProvider.GetById(userId, cancellationToken)
                .ConfigureAwait(false);

            var refreshToken = await _tokenProvider
                .GetByTokenString(_refreshUserTokenParameters.RefreshToken, cancellationToken)
                .ConfigureAwait(false);

            if (user == null || refreshToken == null)
            {
                return new RefreshUserTokenDto
                {
                    Result = RefreshUserTokenDto.ResultType.NotFound,
                    ResponseKey = UiLocalizationKey.RefreshUserTokenNotFound,
                    ResponseMessage = UiLocalizationMessage.RefreshUserTokenNotFound
                };
            }

            if (_authTokenHelper.IsAuthTokenExpired(refreshToken))
            {
                await _tokenProvider.Delete(refreshToken.Id, cancellationToken)
                    .ConfigureAwait(false);
                return new RefreshUserTokenDto
                {
                    Result = RefreshUserTokenDto.ResultType.Expired,
                    ResponseKey = UiLocalizationKey.RefreshUserTokenExpired,
                    ResponseMessage = UiLocalizationMessage.RefreshUserTokenExpired
                };
            }

            var newJwtToken = _authTokenHelper.GenerateJwtToken(principal.Claims);
            await _tokenProvider.Update(refreshToken.Id,
                AuthTokenUpdateDefinitions.ExpirationTime(DateTime.UtcNow.AddHours(1)),
                cancellationToken).ConfigureAwait(false);

            return new RefreshUserTokenDto
            {
                Result = RefreshUserTokenDto.ResultType.Success,
                ResponseKey = UiLocalizationKey.Success,
                ResponseMessage = UiLocalizationMessage.SuccessfulRequest,
                Token = newJwtToken,
                RefreshToken = refreshToken.TokenString
            };
        }
    }
}