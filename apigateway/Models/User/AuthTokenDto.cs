using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public class AuthTokenDto : RestResponse
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string RefreshToken { get; set; }
    }
}