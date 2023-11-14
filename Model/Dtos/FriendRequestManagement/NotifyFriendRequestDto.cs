using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.FriendRequestManagement
{
    public class NotifyFriendRequestDto
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        [JsonPropertyName("user_class")]
        public string UserClass { get; set; } = null;
    }
}