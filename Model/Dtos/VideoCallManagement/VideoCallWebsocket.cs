using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.VideoCallManagement
{
    public class VideoCallWebsocket
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        //Key is user id, value is peer id
        [JsonPropertyName("participants")]
        public ConcurrentDictionary<string, string> Participants { get; set; }

        public VideoCallWebsocket()
        {
            Participants = new();
        }
    }
}