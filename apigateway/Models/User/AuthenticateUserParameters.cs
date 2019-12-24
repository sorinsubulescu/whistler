using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public sealed class AuthenticateUserParameters
    {
        [DataMember]
        [Required(ErrorMessage = "User name can not be empty")]
        public string UserName { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Password can not be empty")]
        public string Password { get; set; }
    }
}