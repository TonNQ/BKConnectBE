using System.Text.Json.Serialization;
using BKConnectBE.Model.Dtos.FriendRequestManagement;

namespace BKConnectBE.Model.Dtos.NotificationManagement
{
    public class SendNotificationDto
    {
        [JsonPropertyName("notification_type")]
        public string NotificationType { get; set; }

        [JsonPropertyName("receiver_id")]
        public string ReceiverId { get; set; }

        [JsonPropertyName("file_id")]
        public long FileId { get; set; }

        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }
    }
}