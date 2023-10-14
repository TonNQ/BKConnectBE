using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.RoomManagement
{
    public class RoomSidebarDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("room_type")]
        public string RoomType { get; set; }
        
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
        
        [JsonPropertyName("last_message")]
        public string LastMessage { get; set; }
        
        [JsonPropertyName("last_message_time")]
        public DateTime LastMessageTime { get; set; }

        [JsonPropertyName("is_read")]
        public bool IsRead { get; set; }
    }
}