using System.Runtime.Serialization;

namespace apigateway
{
    [DataContract]
    public class RestResponse
    {
        internal enum ResultType
        {
            Success = 0,
            Error = 1
        }

        internal ResultType Result { get; private set; }

        [DataMember]
        public string ResponseKey { get; set; }

        [DataMember]
        public string ResponseMessage { get; set; }

        internal static RestResponse Success => new RestResponse
        {
            Result = ResultType.Success,
            ResponseKey = UiLocalizationKey.Success,
            ResponseMessage = UiLocalizationMessage.SuccessfulRequest
        };

        internal static RestResponse Error => new RestResponse
        {
            Result = ResultType.Error,
            ResponseKey = UiLocalizationKey.AnErrorOccurred,
            ResponseMessage = UiLocalizationMessage.AnErrorOccurred
        };
    }
}
