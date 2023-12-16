using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.UploadedFileManagement
{
    public class UploadedFileDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("upload_time")]
        public DateTime UploadTime { get; set; }

        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}