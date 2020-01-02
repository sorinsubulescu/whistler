using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class UnfollowUserCommand : IUnfollowUserCommand
    {
        private readonly UnfollowUserParameters _unfollowUserParameters;
        private readonly string _userId;
        private readonly IUserProvider _userProvider;

        public UnfollowUserCommand(UnfollowUserParameters unfollowUserParameters, string userId, IUserProvider userProvider)
        {
            _unfollowUserParameters =
                unfollowUserParameters ?? throw new ArgumentNullException(nameof(unfollowUserParameters));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var userToUnfollow = await _userProvider.GetById(_unfollowUserParameters.UserToUnfollowId, cancellationToken)
                .ConfigureAwait(false);

            if (userToUnfollow == null)
            {
                return RestResponse.Error;
            }

            await _userProvider
                .Update(_userId, UserUpdateDefinitions.RemoveFollowingUser(userToUnfollow.Id), cancellationToken)
                .ConfigureAwait(false);

            await _userProvider
                .Update(userToUnfollow.Id, UserUpdateDefinitions.RemoveFollowerUser(_userId), cancellationToken)
                .ConfigureAwait(false);

            return RestResponse.Success;
        }
    }
}