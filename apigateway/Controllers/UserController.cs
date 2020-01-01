using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apigateway
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public sealed class UserController : ControllerBase
    {
        private readonly ICommandFactory _commandFactory;
        private readonly IQueryFactory _queryFactory;

        public UserController(ICommandFactory commandFactory, IQueryFactory queryFactory)
        {
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            _queryFactory = queryFactory ?? throw new ArgumentNullException(nameof(queryFactory));
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

        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userDto = await _queryFactory.GetUserQuery(userId).Execute(cancellationToken)
                .ConfigureAwait(false);

            return Ok(userDto);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUserById(string userId,
            CancellationToken cancellationToken = default)
        {
            var userDto = await _queryFactory.GetUserQuery(userId).Execute(cancellationToken)
                .ConfigureAwait(false);

            return Ok(userDto);
        }

        [HttpPut("profile_picture")]
        public async Task<ActionResult> EditProfilePicture(IFormFile file, CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _commandFactory.EditProfilePictureCommand(file, userId).Execute(cancellationToken).ConfigureAwait(false);

            switch (result.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok();
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPatch]
        public async Task<ActionResult> EditUser(EditUserParameters editUserParameters,
            CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _commandFactory.EditUserCommand(editUserParameters, userId).Execute(cancellationToken)
                .ConfigureAwait(false);

            switch (result.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok();
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost("follow")]
        public async Task<ActionResult> FollowUser(FollowUserParameters followUserParameters,
            CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _commandFactory.FollowUserCommand(followUserParameters, userId)
                .Execute(cancellationToken).ConfigureAwait(false);

            switch (result.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok();
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost("unfollow")]
        public async Task<ActionResult> UnfollowUser(UnfollowUserParameters unfollowUserParameters,
            CancellationToken cancellationToken = default)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _commandFactory.UnfollowUserCommand(unfollowUserParameters, userId)
                .Execute(cancellationToken).ConfigureAwait(false);

            switch (result.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok();
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet("brief_info/{userId}")]
        public async Task<ActionResult<UserBriefInfoDto>> GetUserBriefInfo(string userId, CancellationToken cancellationToken = default)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userBriefInfoDto = await _queryFactory.GetUserBriefInfoQuery(userId, currentUserId).Execute(cancellationToken)
                .ConfigureAwait(false);

            switch (userBriefInfoDto.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok(userBriefInfoDto);
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<SearchUsersDto>> SearchUsers(string searchTerm,
            CancellationToken cancellationToken = default)
        {
            var matchingUsers =
                await _queryFactory.SearchUsersQuery(searchTerm).Execute(cancellationToken).ConfigureAwait(false);

            switch (matchingUsers.Result)
            {
                case RestResponse.ResultType.Success:
                    return Ok(matchingUsers);
                case RestResponse.ResultType.Error:
                    return BadRequest();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}