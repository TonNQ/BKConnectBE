using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BKConnectBE.Common.Enumeration;

namespace BKConnectBE.Model.Dtos.MessageManagement
{
    public class SendMessageDto
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("message_type")]
        [EnumDataType(typeof(MessageType))]
        public string TypeOfMessage { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("root_message_id")]
        public long? RootMessageId { get; set; } = null;
    }
}