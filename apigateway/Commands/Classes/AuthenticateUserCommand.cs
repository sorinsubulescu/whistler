using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    internal sealed class AuthenticateUserCommand : IAuthenticateUserCommand
    {
        private readonly IUserProvider _userProvider;
        private readonly IRefreshTokenProvider _tokenProvider;
        private readonly AuthenticateUserParameters _authenticateUserParameters;
        private readonly IAuthTokenHelper _authTokenHelper;

        public AuthenticateUserCommand(IUserProvider userProvider, IRefreshTokenProvider tokenProvider,
            AuthenticateUserParameters authenticateUserParameters, IAuthTokenHelper authTokenHelper)
        {
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            _authenticateUserParameters = authenticateUserParameters ??
                                          throw new ArgumentNullException(nameof(authenticateUserParameters));
            _authTokenHelper = authTokenHelper ?? throw new ArgumentNullException(nameof(authTokenHelper));
        }

        public async Task<AuthenticateUserDto> Execute(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await GetUser(cancellationToken);
            if (user == null)
            {
                return new AuthenticateUserDto
                {
                    Result = AuthenticateUserDto.ResultType.FailedInvalidUserNameOrPassword,
                    ResponseKey = UiLocalizationKey.AuthenticationFailedInvalidUserNameOrPassword,
                    ResponseMessage = UiLocalizationMessage.AuthenticationFailedInvalidUserNameOrPassword
                };
            }

            if (!VerifyPasswordHash(_authenticateUserParameters.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new AuthenticateUserDto
                {
                    Result = AuthenticateUserDto.ResultType.FailedInvalidUserNameOrPassword,
                    ResponseKey = UiLocalizationKey.AuthenticationFailedInvalidUserNameOrPassword,
                    ResponseMessage = UiLocalizationMessage.AuthenticationFailedInvalidUserNameOrPassword
                };
            }

            var newJwtToken = _authTokenHelper.GenerateJwtToken(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            });
            var newRefreshToken = _authTokenHelper.GenerateRefreshToken(user.Id);
            await _tokenProvider.Add(newRefreshToken, cancellationToken)
                .ConfigureAwait(false);

            return new AuthenticateUserDto
            {
                CurrentUser = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    ProfilePictureFileName = user.ProfilePictureFileName
                },
                Result = AuthenticateUserDto.ResultType.Success,
                ResponseKey = UiLocalizationKey.Success,
                ResponseMessage = UiLocalizationMessage.SuccessfulRequest,
                Token = newJwtToken,
                RefreshToken = newRefreshToken.TokenString
            };
        }

        private async Task<User> GetUser(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _userProvider.GetByUserName(_authenticateUserParameters.UserName, cancellationToken)
                       .ConfigureAwait(false) ??
                   await _userProvider.GetByEmail(_authenticateUserParameters.UserName, cancellationToken)
                       .ConfigureAwait(false);
        }

        private static bool VerifyPasswordHash(string password, IReadOnlyCollection<byte> storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (storedHash.Count != 64)
                throw new ArgumentException("Invalid password hash.", nameof(password));

            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid password salt.", nameof(password));

            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}