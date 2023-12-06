using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.NotificationManagement
{
    public class NotifyRoomMessage
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("room_name")]
        public string RoomName { get; set; }

        [JsonPropertyName("room_type")]
        public string RoomType { get; set; }
    }
}