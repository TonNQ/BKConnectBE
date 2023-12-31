using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.VideoCallManagement
{
    public class ParticipantIdInRoom
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("peer_id")]
        public string PeerId { get; set; }

        [JsonPropertyName("web_socket_id")]
        public string WebSocketId { get; set; }
    }
}