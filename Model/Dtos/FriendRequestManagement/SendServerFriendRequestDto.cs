using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.FriendRequestManagement
{
    public class SendServerFriendRequestDto
    {
        [JsonPropertyName("send_time")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("receiver_id")]
        public string ReceiverId { get; set; }
    }
}