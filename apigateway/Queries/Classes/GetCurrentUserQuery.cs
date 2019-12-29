using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class GetUserQuery : IGetUserQuery
    {
        private readonly string _userId;
        private readonly IUserProvider _userProvider;

        public GetUserQuery(string userId, IUserProvider userProvider)
        {
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<UserDto> Execute(CancellationToken cancellationToken = default)
        {
            var user = await _userProvider.GetById(_userId, cancellationToken).ConfigureAwait(false);

            return new UserDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Id = user.Id,
                ProfilePictureFileName = user.ProfilePictureFileName
            };
        }
    }
}