using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.FriendRequestManagement
{
    public class FriendRequestDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("send_time")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }

        [JsonPropertyName("receiver_id")]
        public string ReceiverId { get; set; }
    }
}