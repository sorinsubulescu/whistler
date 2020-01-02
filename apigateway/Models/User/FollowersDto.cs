using System.Collections.Generic;
using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public class FollowersDto : RestResponse
    {
        [DataMember]
        public IEnumerable<UserDto> Followers { get; set; }
    }
}