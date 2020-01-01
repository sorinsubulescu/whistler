using System.Collections.Generic;
using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public class SearchUsersDto : RestResponse
    {
        [DataMember]
        public IEnumerable<UserDto> MatchingUsers { get; set; }
    }
}