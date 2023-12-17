using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.NotificationManagement
{
    public class ReceiveNotificationDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("notification_type")]
        public string NotificationType { get; set; }

        [JsonPropertyName("sender_id")]
        public string SenderId { get; set; }

        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; }

        [JsonPropertyName("send_time")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("is_read")]
        public bool IsRead { get; set; }

        [JsonPropertyName("room_message")]
        public NotifyRoomMessage RoomMessage { get; set; }

        [JsonPropertyName("post_file")]
        public NotifyPostFile PostFile { get; set; }
    }
}