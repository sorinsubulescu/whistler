using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class SearchUsersQuery : ISearchUsersQuery
    {
        private readonly string _searchTerm;
        private readonly IUserProvider _userProvider;

        public SearchUsersQuery(string searchTerm, IUserProvider userProvider)
        {
            _searchTerm = searchTerm ?? throw new ArgumentNullException(nameof(searchTerm));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<SearchUsersDto> Execute(CancellationToken cancellationToken = default)
        {
            var matchingUsers = await _userProvider.GetAllByFullNameSearchTerm(_searchTerm, cancellationToken)
                .ConfigureAwait(false);

            var matchingUsersDto = matchingUsers.Select(
                user => new UserDto
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    Id = user.Id,
                    ProfilePictureFileName = user.ProfilePictureFileName
                });

            return new SearchUsersDto
            {
                MatchingUsers = matchingUsersDto
            };
        }
    }
}