using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public sealed class RegisterUserDto : RestResponse
    {
        internal new enum ResultType
        {
            Success = 0,
            FailedUserNameUsed = 1,
            FailedPasswordInvalid = 2,
            FailedCanNotCreateUser = 3
        }

        internal new ResultType Result { get; set; }
    }
}