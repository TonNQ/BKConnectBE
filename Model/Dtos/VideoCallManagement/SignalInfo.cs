using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.VideoCallManagement
{
    public class SignalInfo
    {
        [JsonPropertyName("signal_type")]
        public string SignalType { get; set; }

        [JsonPropertyName("to")]
        public string ToUser { get; set; }

        [JsonPropertyName("signal")]
        public SignalObject SignalObject { get; set; }
    }
}