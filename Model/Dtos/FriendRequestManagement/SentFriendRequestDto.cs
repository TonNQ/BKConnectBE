using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.FriendRequestManagement
{
    public class SentFriendRequestDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        
        [JsonPropertyName("send_time")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("accepted_time")]
        public DateTime AcceptedTime { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("receiver_id")]
        public string ReceiverId { get; set; }

        [JsonPropertyName("receiver_name")]
        public string ReceiverName { get; set; }

        [JsonPropertyName("receiver_avatar")]
        public string ReceiverAvatar { get; set; }

        [JsonPropertyName("receiver_class_name")]
        public string ReceiverClassName { get; set; }
    }
}