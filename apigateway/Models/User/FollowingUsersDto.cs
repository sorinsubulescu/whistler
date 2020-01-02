using System.Collections.Generic;
using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public class FollowingUsersDto : RestResponse
    {
        [DataMember]
        public IEnumerable<UserDto> FollowingUsers { get; set; }
    }
}