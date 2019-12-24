using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public sealed class RefreshUserTokenDto : AuthTokenDto
    {
        internal new enum ResultType
        {
            Success = 0,
            Failed = 1,
            NotFound = 2,
            Expired = 3
        }

        internal new ResultType Result { get; set; }
    }
}