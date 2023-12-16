using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.ClassFileManagement
{
    public class AddFileDto
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }
    }
}