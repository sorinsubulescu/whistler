using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public sealed class RegisterUserParameters
    {
        [DataMember]
        [Required(ErrorMessage = "Full name can not be empty")]
        public string FullName { get; set; }

        [DataMember]
        [Required(ErrorMessage = "User name can not be empty")]
        public string UserName { get; set; }

        [DataMember]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
        public string Password { get; set; }

        [DataMember]
        [EmailAddress(ErrorMessage = "Please provide a valid email")]
        public string Email { get; set; }
    }
}