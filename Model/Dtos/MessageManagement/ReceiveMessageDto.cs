using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BKConnectBE.Common.Enumeration;

namespace BKConnectBE.Model.Dtos.MessageManagement
{
    public class ReceiveMessageDto
    {
        [JsonPropertyName("temp_id")]
        public string TempId { get; set; }

        [JsonPropertyName("message_id")]
        public long Id { get; set; }

        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }

        [JsonPropertyName("sender_avatar")]
        public string SenderAvatar { get; set; }

        [JsonPropertyName("send_time")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("message_type")]
        [EnumDataType(typeof(MessageType))]
        public string TypeOfMessage { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("last_message")]
        public string LastMessage { get; set; }

        [JsonPropertyName("root_sender")]
        public string RootSender { get; set; }

        [JsonPropertyName("root_message_content")]
        public string RootMessageContent { get; set; }
    }
}