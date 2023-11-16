using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Model.Dtos.NotificationManagement;

namespace BKConnectBE.Model.Dtos.ChatManagement
{
    public class ReceiveWebSocketData
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("data_type")]
        [EnumDataType(typeof(WebSocketDataType))]
        public string DataType { get; set; }

        [JsonPropertyName("message")]
        public ReceiveMessageDto Message { get; set; } = null;

        [JsonPropertyName("notification")]
        public ReceiveNotificationDto Notification { get; set; } = null;
    }
}