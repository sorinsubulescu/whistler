using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class FollowUserCommand : IFollowUserCommand
    {
        private readonly FollowUserParameters _followUserParameters;
        private readonly string _userId;
        private readonly IUserProvider _userProvider;

        public FollowUserCommand(FollowUserParameters followUserParameters, string userId, IUserProvider userProvider)
        {
            _followUserParameters =
                followUserParameters ?? throw new ArgumentNullException(nameof(followUserParameters));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var userToFollow = await _userProvider.GetById(_followUserParameters.UserToFollowId, cancellationToken)
                .ConfigureAwait(false);

            if (userToFollow == null)
            {
                return RestResponse.Error;
            }

            var user = await _userProvider.GetById(_userId, cancellationToken).ConfigureAwait(false);

            if (user.FollowingUserIds.Any(e => e == userToFollow.Id))
            {
                return RestResponse.Error;;
            }

            await _userProvider
                .Update(_userId, UserUpdateDefinitions.AddFollowingUser(userToFollow.Id), cancellationToken)
                .ConfigureAwait(false);

            await _userProvider
                .Update(userToFollow.Id, UserUpdateDefinitions.AddFollowerUser(_userId), cancellationToken)
                .ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}