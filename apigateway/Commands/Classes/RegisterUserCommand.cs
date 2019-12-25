using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    internal sealed class RegisterUserCommand : IRegisterUserCommand
    {
        private readonly IUserProvider _userProvider;
        private readonly RegisterUserParameters _registerUserParameters;

        public RegisterUserCommand(IUserProvider userProvider, RegisterUserParameters registerUserParameters)
        {
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            _registerUserParameters = registerUserParameters ?? throw new ArgumentNullException(nameof(registerUserParameters));
        }

        public async Task<RegisterUserDto> Execute(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userCount = await _userProvider.CountAll(cancellationToken).ConfigureAwait(false);

            if (userCount > 0)
            {
                return new RegisterUserDto
                {
                    Result = RegisterUserDto.ResultType.FailedCanNotCreateUser,
                    ResponseKey = UiLocalizationKey.RegisterFailedCannotCreateUser,
                    ResponseMessage = UiLocalizationMessage.RegisterFailedCannotCreateUser
                };
            }

            var user = CreateUser();

            if (await IsUserNameUsed(user, cancellationToken)
                .ConfigureAwait(false))
            {
                return new RegisterUserDto
                {
                    Result = RegisterUserDto.ResultType.FailedUserNameUsed,
                    ResponseKey = UiLocalizationKey.RegisterFailedUserNameUsed,
                    ResponseMessage = UiLocalizationMessage.RegisterFailedUserNameUsed
                };
            }

            if (await IsEmailUsed(user, cancellationToken)
                .ConfigureAwait(false))
            {
                return new RegisterUserDto
                {
                    Result = RegisterUserDto.ResultType.FailedUserNameUsed,
                    ResponseKey = UiLocalizationKey.RegisterFailedEmailUsed,
                    ResponseMessage = UiLocalizationMessage.RegisterFailedEmailUsed
                };
            }

            CreatePasswordHash(_registerUserParameters.Password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userProvider.Add(user, cancellationToken).ConfigureAwait(false);

            return new RegisterUserDto
            {
                Result = RegisterUserDto.ResultType.Success,
                ResponseKey = UiLocalizationKey.RegisterUserSuccess,
                ResponseMessage = UiLocalizationMessage.AccountSuccessfullyCreated
            };
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private User CreateUser() =>
            new User
            {
                FullName = _registerUserParameters.FullName,
                UserName = _registerUserParameters.UserName,
                Email = _registerUserParameters.Email
            };

        private async Task<bool> IsUserNameUsed(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userFromDb = await _userProvider.GetByUserName(user.UserName, cancellationToken)
                .ConfigureAwait(false);
            return userFromDb != null;
        }

        private async Task<bool> IsEmailUsed(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userFromDb = await _userProvider.GetByEmail(user.Email, cancellationToken)
                .ConfigureAwait(false);
            return userFromDb != null;
        }
    }
}