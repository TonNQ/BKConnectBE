using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.FriendRequestManagement
{
    public class ReceivedFriendRequestDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("send_time")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }

        [JsonPropertyName("sender_avatar")]
        public string SenderAvatar { get; set; }

        [JsonPropertyName("sender_class_name")]
        public string SenderClassName { get; set; }
    }
}