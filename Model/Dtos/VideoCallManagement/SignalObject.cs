using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.VideoCallManagement
{
    public class SignalObject
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("sdp")]
        public string Sdp { get; set; }
    }
}