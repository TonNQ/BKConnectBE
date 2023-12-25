using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.VideoCallManagement
{
    public class VideoCallData
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("video_call_type")]
        public string VideoCallType { get; set; }

        [JsonPropertyName("participants")]
        public List<ParticipantInfo> Participants { get; set; }

        public VideoCallData()
        {
            Participants = new();
        }
    }
}