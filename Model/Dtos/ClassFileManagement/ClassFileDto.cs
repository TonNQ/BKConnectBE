using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.ClassFileManagement
{
    public class ClassFileDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}