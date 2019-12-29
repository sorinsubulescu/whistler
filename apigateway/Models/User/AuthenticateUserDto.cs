using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public sealed class AuthenticateUserDto : AuthTokenDto
    {
        [DataMember]
        public UserDto CurrentUser { get; set; }

        internal new ResultType Result { get; set; }

        internal new enum ResultType
        {
            Success = 0,
            FailedInvalidUserNameOrPassword = 1
        }
    }
}