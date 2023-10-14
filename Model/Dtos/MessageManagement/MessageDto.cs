using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.MessageManagement
{
    public class MessageDto
    {
        [JsonPropertyName("message_id")]
        public long Id { get; set; }

        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }

        [JsonPropertyName("send_time")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("type_message")]
        public string TypeOfMessage { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("root_message_id")]
        public long RootMessageId { get; set; }

        [JsonPropertyName("root_message_content")]
        public string RootMessageContent { get; set; }
    }
}