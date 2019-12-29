using System;
using System.Threading;
using System.Threading.Tasks;

namespace apigateway
{
    public class EditUserCommand : IEditUserCommand
    {
        private readonly EditUserParameters _editUserParameters;
        private readonly string _userId;
        private readonly IUserProvider _userProvider;

        public EditUserCommand(EditUserParameters editUserParameters, string userId, IUserProvider userProvider)
        {
            _editUserParameters = editUserParameters ?? throw new ArgumentNullException(nameof(editUserParameters));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var isEditSuccessful = true;

            if (!string.IsNullOrEmpty(_editUserParameters.FullName))
            {
                var isFullNameUpdateSuccessful = await _userProvider
                    .Update(_userId, UserUpdateDefinitions.FullName(_editUserParameters.FullName), cancellationToken)
                    .ConfigureAwait(false);

                if (!isFullNameUpdateSuccessful)
                {
                    isEditSuccessful = false;
                }
            }

            return !isEditSuccessful ? RestResponse.Error : RestResponse.Success;
        }
    }
}