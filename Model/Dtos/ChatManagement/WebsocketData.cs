using System.Text.Json.Serialization;
using BKConnectBE.Model.Dtos.FriendRequestManagement;
using BKConnectBE.Model.Dtos.MessageManagement;

namespace BKConnectBE.Model.Dtos.ChatManagement
{
    public class WebsocketData
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("data_type")]
        public string DataType { get; set; }

        [JsonPropertyName("message")]
        public MessageDto Message { get; set; } = null;

        [JsonPropertyName("friend_request")]
        public FriendRequestDto FriendRequest { get; set; } = null;
    }
}