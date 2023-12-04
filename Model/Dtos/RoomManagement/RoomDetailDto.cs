using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.RoomManagement
{
    public class RoomDetailDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

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
    }
}