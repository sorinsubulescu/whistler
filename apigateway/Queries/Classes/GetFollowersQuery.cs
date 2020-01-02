using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class GetFollowersQuery : IGetFollowersQuery
    {
        private readonly string _userId;
        private readonly IUserProvider _userProvider;

        public GetFollowersQuery(string userId, IUserProvider userProvider)
        {
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<FollowersDto> Execute(CancellationToken cancellationToken = default)
        {
            var user = await _userProvider.GetById(_userId, cancellationToken).ConfigureAwait(false);

            var followers = new List<UserDto>();

            foreach (var followerUserId in user.FollowerUserIds)
            {
                var follower = await _userProvider.GetById(followerUserId, cancellationToken).ConfigureAwait(false);
                followers.Add(new UserDto
                {
                    Email = follower.Email,
                    FullName = follower.FullName,
                    Id = follower.Id,
                    ProfilePictureFileName = follower.ProfilePictureFileName
                });
            }

            return new FollowersDto
            {
                Followers = followers
            };
        }
    }
}