using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.UploadedFileManagement
{
    public class AddFileDto
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }
    }
}