using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public class UserBriefInfoDto : RestResponse
    {
        [DataMember]
        public int FollowingCount { get; set; }

        [DataMember]
        public int FollowersCount { get; set; }

        [DataMember]
        public bool IsFollowedByMe { get; set; }
    }
}