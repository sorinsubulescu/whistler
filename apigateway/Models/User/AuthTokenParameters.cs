using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public class AuthTokenParameters
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string RefreshToken { get; set; }
    }
}