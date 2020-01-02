using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class GetFollowingUsersQuery : IGetFollowingUsersQuery
    {
        private readonly string _userId;
        private readonly IUserProvider _userProvider;

        public GetFollowingUsersQuery(string userId, IUserProvider userProvider)
        {
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<FollowingUsersDto> Execute(CancellationToken cancellationToken = default)
        {
            var user = await _userProvider.GetById(_userId, cancellationToken).ConfigureAwait(false);

            var followingUsers = new List<UserDto>();

            foreach (var followingUserId in user.FollowingUserIds)
            {
                var followingUser = await _userProvider.GetById(followingUserId, cancellationToken).ConfigureAwait(false);
                followingUsers.Add(new UserDto
                {
                    Email = followingUser.Email,
                    FullName = followingUser.FullName,
                    Id = followingUser.Id,
                    ProfilePictureFileName = followingUser.ProfilePictureFileName
                });
            }

            return new FollowingUsersDto
            {
                FollowingUsers = followingUsers
            };
        }
    }
}