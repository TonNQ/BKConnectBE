using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.VideoCallManagement
{
    public class VideoCallWebsocket
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("user_ids")]
        public List<string> UserIds { get; set; } = new();
    }
}