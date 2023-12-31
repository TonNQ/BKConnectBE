using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.VideoCallManagement
{
    public class VideoCallWebsocket
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        //Key is ws id, value is peer id
        [JsonPropertyName("participants")]
        public List<ParticipantIdInRoom> Participants { get; set; }

        public VideoCallWebsocket()
        {
            Participants = new();
        }
    }
}