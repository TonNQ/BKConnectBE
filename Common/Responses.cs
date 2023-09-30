using System.Text.Json.Serialization;
using BKConnectBE.Common;

namespace BKConnect.BKConnectBE.Common
{
    public class Responses
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }

        public Responses()
        {
            Code = ResponseCode.OK;
        }
    }
}