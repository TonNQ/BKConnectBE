using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.MessageManagement
{
    public class FileDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}