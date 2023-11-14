using System.Text.Json.Serialization;
using BKConnectBE.Model.Dtos.FriendRequestManagement;

namespace BKConnectBE.Model.Dtos.NotificationManagement
{
    public class ReceiveNotificationDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("sender_time")]
        public DateTime SendTime { get; set; }

        [JsonPropertyName("notification_type")]
        public string NotificationType { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("is_read")]
        public bool IsRead { get; set; }

        [JsonPropertyName("friend_request")]
        public NotifyFriendRequestDto FriendRequest { get; set; } = null;
    }
}