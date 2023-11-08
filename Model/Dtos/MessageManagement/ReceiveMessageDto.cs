using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BKConnectBE.Common.Enumeration;

namespace BKConnectBE.Model.Dtos.MessageManagement
{
    public class ReceiveMessageDto
    {
        [JsonPropertyName("message_id")]
        public long Id { get; set; }

        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }

        [JsonPropertyName("sender_avatar")]
        public string SenderAvatar { get; set; }

        [JsonPropertyName("send_time")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("type_message")]
        [EnumDataType(typeof(MessageType))]
        public string TypeOfMessage { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("root_sender")]
        public string RootSender { get; set; }

        [JsonPropertyName("root_message_content")]
        public string RootMessageContent { get; set; }
    }
}