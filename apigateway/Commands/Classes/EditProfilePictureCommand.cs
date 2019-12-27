using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace apigateway
{
    public class EditProfilePictureCommand : IEditProfilePictureCommand
    {
        private readonly IFormFile _file;
        private readonly string _userId;
        private readonly IImageWriter _imageWriter;
        private readonly IUserProvider _userProvider;

        private const string ProfilePicturesFolderPath = "wwwroot//profile";

        public EditProfilePictureCommand(IFormFile file, string userId, IImageWriter imageWriter, IUserProvider userProvider)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
            _userId = userId ?? throw new ArgumentNullException(nameof(userId));
            _imageWriter = imageWriter ?? throw new ArgumentNullException(nameof(imageWriter));
            _userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        public async Task<RestResponse> Execute(CancellationToken cancellationToken = default)
        {
            var fileName = await _imageWriter.UploadImage(_file, ProfilePicturesFolderPath).ConfigureAwait(false);

            var updateSuccessful = await _userProvider
                .Update(_userId, UserUpdateDefinitions.ProfilePictureFileName(fileName), cancellationToken)
                .ConfigureAwait(false);

            return updateSuccessful ? RestResponse.Success : RestResponse.Error;
        }
    }
}