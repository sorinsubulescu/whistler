using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apigateway
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public sealed class UserController : ControllerBase
    {
        private readonly ICommandFactory _commandFactory;

        public UserController(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(
            [FromBody] RegisterUserParameters registerUserParameters,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _commandFactory
                .RegisterUserCommand(registerUserParameters)
                .Execute(cancellationToken).ConfigureAwait(false);
            switch (result.Result)
            {
                case RegisterUserDto.ResultType.FailedPasswordInvalid:
                case RegisterUserDto.ResultType.FailedUserNameUsed:
                case RegisterUserDto.ResultType.FailedCanNotCreateUser:
                    return BadRequest(result);
                case RegisterUserDto.ResultType.Success:
                    return Ok(result);
                default:
                    throw new ArgumentOutOfRangeException(nameof(result.Result));
            }
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticateUserDto>> Authenticate(
            [FromBody] AuthenticateUserParameters authenticateUserParameters,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _commandFactory
                .AuthenticateUserCommand(authenticateUserParameters)
                .Execute(cancellationToken).ConfigureAwait(false);
            switch (result.Result)
            {
                case AuthenticateUserDto.ResultType.FailedInvalidUserNameOrPassword:
                    return BadRequest(result);
                case AuthenticateUserDto.ResultType.Success:
                    return Ok(result);
                default:
                    throw new ArgumentOutOfRangeException(nameof(result.Result));
            }
        }

        [HttpPost("refreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult> Refresh(
            [FromBody] RefreshUserTokenParameters refreshUserParameters,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _commandFactory
                .RefreshUserTokenCommand(refreshUserParameters)
                .Execute(cancellationToken).ConfigureAwait(false);

            switch (result.Result)
            {
                case RefreshUserTokenDto.ResultType.Success:
                    return Ok(result);
                case RefreshUserTokenDto.ResultType.Failed:
                case RefreshUserTokenDto.ResultType.Expired:
                    return BadRequest(result);
                case RefreshUserTokenDto.ResultType.NotFound:
                    return NotFound(result);

                default:
                    throw new ArgumentOutOfRangeException(nameof(result.Result));
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout(
            [FromBody] LogoutUserParameters logoutUserParameters,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _commandFactory
                .LogoutUserCommand(logoutUserParameters)
                .Execute(cancellationToken).ConfigureAwait(false);

            if (result.Result == RestResponse.ResultType.Error)
                BadRequest(result);

            return Ok(result);
        }
    }
}