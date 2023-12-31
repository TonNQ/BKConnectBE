using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.RoomManagement
{
    public class RoomDetailDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("room_type")]
        public string RoomType { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("last_online")]
        public DateTime? LastOnline { get; set; }

        [JsonPropertyName("total_member")]
        public int? TotalMember { get; set; }

        [JsonPropertyName("is_online")]
        public bool IsOnline { get; set; }

        [JsonPropertyName("friend_id")]
        public string FriendId { get; set; }

        [JsonPropertyName("last_message_id")]
        public long? LastMessageId { get; set; }

        [JsonPropertyName("last_message")]
        public string LastMessage { get; set; }

        [JsonPropertyName("last_message_time")]
        public DateTime LastMessageTime { get; set; }

        [JsonPropertyName("is_read")]
        public bool IsRead { get; set; }
    }
}