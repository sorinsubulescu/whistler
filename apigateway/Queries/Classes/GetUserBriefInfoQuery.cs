using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class GetUserBriefInfoQuery : IGetUserBriefInfoQuery
    {
        private readonly string _userId;
        private readonly string _currentUserId;
        private readonly IUserProvider _userProvider;

        public GetUserBriefInfoQuery(string userId, string currentUserId, IUserProvider userProvider)
        {
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _currentUserId = currentUserId ?? throw new ArgumentNullException(nameof(currentUserId));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<UserBriefInfoDto> Execute(CancellationToken cancellationToken = default)
        {
            var user = await _userProvider.GetById(_userId, cancellationToken).ConfigureAwait(false);

            if (user == null)
            {
                return (UserBriefInfoDto) RestResponse.Error;
            }

            return new UserBriefInfoDto
            {
                FollowersCount = user.FollowerUserIds.Count(),
                FollowingCount = user.FollowingUserIds.Count(),
                IsFollowedByMe = user.FollowerUserIds.Any(e => e == _currentUserId)
            };
        }
    }
}