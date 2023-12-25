using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Model.Dtos.NotificationManagement;
using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Model.Dtos.VideoCallManagement;

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

        [JsonPropertyName("changed_room_info")]
        public ChangedRoomDto ChangedRoomInfo { get; set; } = null;

        [JsonPropertyName("video_call")]
        public VideoCallData VideoCall { get; set; }

        [JsonPropertyName("room_info")]
        public RoomDetailDto RoomInfo { get; set; }

        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; } = null;
    }
}