using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.NotificationManagement
{
    public class NotifyPostFile
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("room_name")]
        public string RoomName { get; set; }

        [JsonPropertyName("file_id")]
        public long FileId { get; set; }

        [JsonPropertyName("file_name")]
        public string FileName { get; set; }
    }
}